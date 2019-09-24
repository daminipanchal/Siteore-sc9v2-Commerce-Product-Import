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
    [PipelineDisplayName(SellableItemStiboAttributesConstants.Pipelines.Blocks.GetSellableItemStiboAttributeViewBlockForNew)]
    public class GetSellableItemStiboAttributeViewBlockForNew : PipelineBlock<EntityView, EntityView, CommercePipelineExecutionContext>
    {
        private readonly ViewCommander _viewCommander;

        public GetSellableItemStiboAttributeViewBlockForNew(ViewCommander viewCommander)
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
            //Make sure we target the correct View
            if (string.IsNullOrWhiteSpace(request.ViewName) ||
                     !request.ViewName.Equals(catalogViewsPolicy.Master, StringComparison.OrdinalIgnoreCase) &&
                     !request.ViewName.Equals(catalogViewsPolicy.Details, StringComparison.OrdinalIgnoreCase) &&
                     !request.ViewName.Equals(sellableitemstiboattributesviewspolicy.New, StringComparison.OrdinalIgnoreCase) && !isVariationView && !isConnectView)
            {
                return Task.FromResult(arg);
            }

            var sellableItem = (Sitecore.Commerce.Plugin.Catalog.SellableItem)request.Entity;

            // See if we are dealing with the base sellable item or one of its variations.
            var variationId = string.Empty;
            New component;
            if (isVariationView && !string.IsNullOrEmpty(arg.ItemId))
            {
                variationId = arg.ItemId;
                component = sellableItem.GetVariation(variationId).GetComponent<New>();
            }
            else if (!string.IsNullOrEmpty(arg.ItemId) && arg.VersionedItemId != ("-1"))
            {
                variationId = arg.ItemId;
                component = sellableItem.GetVariation(variationId).GetComponent<New>();
            }
            else
            {
                component = sellableItem.GetComponent<New>(variationId);
            }


            var targetView = arg;
            bool isEditView = false;


            #region 30. New
            isEditView = !string.IsNullOrEmpty(arg.Action) && arg.Action.Equals(sellableitemstiboattributesviewspolicy.New, StringComparison.OrdinalIgnoreCase);
            if (!isEditView)
            {
                // Create a new view and add it to the current entity view.
                var view = new EntityView
                {
                    Name = context.GetPolicy<SellableItemStiboAttributesViewsPolicy>().New,
                    DisplayName = "New",
                    EntityId = arg.EntityId,
                    DisplayRank = 7,
                    EntityVersion = arg.EntityVersion,
                    ItemId = variationId,
                    Icon = "piece",
                };

                arg.ChildViews.Add(view);
                targetView = view;
            }

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.US_Launch_Date_MM_DD_YYYY), component.US_Launch_Date_MM_DD_YYYY, component.GetDisplayName(nameof(component.US_Launch_Date_MM_DD_YYYY)));
            #endregion


            return Task.FromResult(arg);
        }
    }
}