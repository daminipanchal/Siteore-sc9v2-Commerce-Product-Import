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
    [PipelineDisplayName(SellableItemStiboAttributesConstants.Pipelines.Blocks.GetSellableItemStiboAttributeViewBlockForProductFeature)]
    public class GetSellableItemStiboAttributeViewBlockForProductFeatures : PipelineBlock<EntityView, EntityView, CommercePipelineExecutionContext>
    {
        private readonly ViewCommander _viewCommander;

        public GetSellableItemStiboAttributeViewBlockForProductFeatures(ViewCommander viewCommander)
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

            // Only proceed if the current entity is a sellable item
            if (!(request.Entity is Sitecore.Commerce.Plugin.Catalog.SellableItem))
            {
                return Task.FromResult(arg);
            }

            // Make sure that we target the correct views
            if (string.IsNullOrEmpty(request.ViewName) ||
                !request.ViewName.Equals(catalogViewsPolicy.Master, StringComparison.OrdinalIgnoreCase) &&
                !request.ViewName.Equals(catalogViewsPolicy.Details, StringComparison.OrdinalIgnoreCase) &&
                !request.ViewName.Equals(sellableitemstiboattributesviewspolicy.ProductFeatures, StringComparison.OrdinalIgnoreCase) &&
                !isVariationView &&
                !isConnectView)
            {
                return Task.FromResult(arg);
            }

            var sellableItem = (Sitecore.Commerce.Plugin.Catalog.SellableItem)request.Entity;

            // See if we are dealing with the base sellable item or one of its variations.
            var variationId = string.Empty;
            ProductFeatures component;
            if (isVariationView && !string.IsNullOrEmpty(arg.ItemId))
            {
                variationId = arg.ItemId;
                component = sellableItem.GetVariation(variationId).GetComponent<ProductFeatures>();
            }
            else if (!string.IsNullOrEmpty(arg.ItemId) && arg.VersionedItemId != ("-1"))
            {
                variationId = arg.ItemId;
                component = sellableItem.GetVariation(variationId).GetComponent<ProductFeatures>();
            }
            else
            {
                component = sellableItem.GetComponent<ProductFeatures>(variationId);
            }

            var targetView = arg;
            bool isEditView = false;


            #region 9. Features
            isEditView = !string.IsNullOrEmpty(arg.Action) && arg.Action.Equals(sellableitemstiboattributesviewspolicy.ProductFeatures, StringComparison.OrdinalIgnoreCase);
            if (!isEditView)
            {
                // Create a new view and add it to the current entity view.
                var view = new EntityView
                {
                    Name = context.GetPolicy<SellableItemStiboAttributesViewsPolicy>().ProductFeatures,
                    DisplayName = "Features",
                    EntityId = arg.EntityId,
                    DisplayRank = 4,
                    EntityVersion = arg.EntityVersion,
                    ItemId = variationId,
                    Icon = "piece",
                };

                arg.ChildViews.Add(view);
                targetView = view;
            }

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Marketing_Claims_1), component.Marketing_Claims_1, component.GetDisplayName(nameof(component.Marketing_Claims_1)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Mirror_Bath_Product_Type), component.Mirror_Bath_Product_Type, component.GetDisplayName(nameof(component.Mirror_Bath_Product_Type)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Mirror_Beveled_Frame_YN), component.Mirror_Beveled_Frame_YN, component.GetDisplayName(nameof(component.Mirror_Beveled_Frame_YN)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Mirror_Frame_Finish_Family), component.Mirror_Frame_Finish_Family, component.GetDisplayName(nameof(component.Mirror_Frame_Finish_Family)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Fitting_Features), component.Fitting_Features, component.GetDisplayName(nameof(component.Fitting_Features)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Feature_Bullets_1), component.Feature_Bullets_1, component.GetDisplayName(nameof(component.Feature_Bullets_1)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Feature_Bullets_2), component.Feature_Bullets_2, component.GetDisplayName(nameof(component.Feature_Bullets_2)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Feature_Bullets_3), component.Feature_Bullets_3, component.GetDisplayName(nameof(component.Feature_Bullets_3)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Feature_Bullets_4), component.Feature_Bullets_4, component.GetDisplayName(nameof(component.Feature_Bullets_4)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Feature_Bullets_5), component.Feature_Bullets_5, component.GetDisplayName(nameof(component.Feature_Bullets_5)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Feature_Bullets_6), component.Feature_Bullets_6, component.GetDisplayName(nameof(component.Feature_Bullets_6)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Feature_Bullets_7), component.Feature_Bullets_7, component.GetDisplayName(nameof(component.Feature_Bullets_7)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Feature_Bullets_8), component.Feature_Bullets_8, component.GetDisplayName(nameof(component.Feature_Bullets_8)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Feature_Bullets_9), component.Feature_Bullets_9, component.GetDisplayName(nameof(component.Feature_Bullets_9)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Feature_Bullets_10), component.Feature_Bullets_10, component.GetDisplayName(nameof(component.Feature_Bullets_10)));
            #endregion


            return Task.FromResult(arg);
        }
    }
}