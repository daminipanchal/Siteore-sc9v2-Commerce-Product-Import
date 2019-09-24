using LX.Commerce.Extensions.Annotation;
using LX.Plugin.SellableItem.StiboAttributes.Components;
using LX.Plugin.SellableItem.StiboAttributes.Policies;
using Serilog;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.EntityViews;
using Sitecore.Commerce.Plugin.Catalog;
using Sitecore.Framework.Conditions;
using Sitecore.Framework.Pipelines;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace LX.Plugin.SellableItem.StiboAttributes.Pipelines.Blocks.EntityViews
{
    [PipelineDisplayName(SellableItemStiboAttributesConstants.Pipelines.Blocks.GetSellableItemStiboAttributeViewBlockForCategoryAttributes)]
    public class GetSellableItemStiboAttributeViewBlockForCategoryAttributes : PipelineBlock<EntityView, EntityView, CommercePipelineExecutionContext>
    {
        private readonly ViewCommander _viewCommander;

        public GetSellableItemStiboAttributeViewBlockForCategoryAttributes(ViewCommander viewCommander)
        {
            this._viewCommander = viewCommander;
        }

        public override Task<EntityView> Run(EntityView arg, CommercePipelineExecutionContext context)
        {
            Condition.Requires(arg).IsNotNull($"{Name}: The argument cannot be null.");

            var request = this._viewCommander.CurrentEntityViewArgument(context.CommerceContext);

            var catalogViewsPolicy = context.GetPolicy<KnownCatalogViewsPolicy>();

            var sellableitemstiboattributesviewspolicy = context.GetPolicy<SellableItemStiboAttributesViewsPolicy>();

            var isVariationView = request.ViewName.Equals(catalogViewsPolicy.Variant, StringComparison.OrdinalIgnoreCase);
            var isConnectView = arg.Name.Equals(catalogViewsPolicy.ConnectSellableItem, StringComparison.OrdinalIgnoreCase);

            // Only proceed if the current entity is a sellable item or Category
            if ((request.Entity is Sitecore.Commerce.Plugin.Catalog.Category) || (request.Entity is Sitecore.Commerce.Plugin.Catalog.SellableItem))
            {
                // Make sure that we target the correct views
                if (string.IsNullOrEmpty(request.ViewName) ||
                    !request.ViewName.Equals(catalogViewsPolicy.Master, StringComparison.OrdinalIgnoreCase) &&
                    !request.ViewName.Equals(catalogViewsPolicy.Details, StringComparison.OrdinalIgnoreCase) &&
                    !request.ViewName.Equals(sellableitemstiboattributesviewspolicy.CategoryAttributes, StringComparison.OrdinalIgnoreCase) &&
                    !isVariationView &&
                    !isConnectView)
                {
                    return Task.FromResult(arg);
                }
                Sitecore.Commerce.Plugin.Catalog.SellableItem sellableItem = null; Sitecore.Commerce.Plugin.Catalog.Category category = null; var variationId = string.Empty;
                if (request.Entity is Sitecore.Commerce.Plugin.Catalog.SellableItem)
                {
                    sellableItem = (Sitecore.Commerce.Plugin.Catalog.SellableItem)request.Entity;
                }
                if (request.Entity is Sitecore.Commerce.Plugin.Catalog.Category)
                {
                    category = (Sitecore.Commerce.Plugin.Catalog.Category)request.Entity;
                }


                CategoryAttributes component = null;
                if (category != null)
                {
                    component = category.GetComponent<CategoryAttributes>();
                }

                // See if we are dealing with the base sellable item or one of its variations.
                if (sellableItem != null)
                {
                    if (isVariationView && !string.IsNullOrEmpty(arg.ItemId))
                    {
                        variationId = arg.ItemId;
                        component = sellableItem.GetVariation(variationId).GetComponent<CategoryAttributes>();
                    }
                    else if (!string.IsNullOrEmpty(arg.ItemId) && arg.VersionedItemId != ("-1"))
                    {
                        variationId = arg.ItemId;
                        component = sellableItem.GetVariation(variationId).GetComponent<CategoryAttributes>();
                    }
                    else
                    {
                        component = sellableItem.GetComponent<CategoryAttributes>(variationId);
                    }
                }

                var targetView = arg;
                bool isEditView = false;

                #region 16. Category Mapping
                isEditView = !string.IsNullOrEmpty(arg.Action) && arg.Action.Equals(sellableitemstiboattributesviewspolicy.CategoryAttributes, StringComparison.OrdinalIgnoreCase);
                if (!isEditView)
                {
                    // Create a new view and add it to the current entity view.
                    var view = new EntityView
                    {
                        Name = context.GetPolicy<SellableItemStiboAttributesViewsPolicy>().CategoryAttributes,
                        DisplayName = "Category Mapping",
                        EntityId = arg.EntityId,
                        DisplayRank = 2,
                        EntityVersion = arg.EntityVersion,
                        ItemId = variationId,
                        Icon = "piece",
                    };

                    arg.ChildViews.Add(view);
                    targetView = view;
                }

                AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.SitecoreMappingPath), component.SitecoreMappingPath, component.GetDisplayName(nameof(component.SitecoreMappingPath)));

                AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.ClassificationType), component.ClassificationType, component.GetDisplayName(nameof(component.ClassificationType)));
                #endregion
            }
            return Task.FromResult(arg);
        }
    }
}