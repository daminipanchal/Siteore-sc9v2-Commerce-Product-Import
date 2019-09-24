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
    [PipelineDisplayName(SellableItemStiboAttributesConstants.Pipelines.Blocks.GetSellableItemStiboAttributesViewBlockForProductOverview)]
    public class GetSellableItemStiboAttributeViewBlockForProductOverview : PipelineBlock<EntityView, EntityView, CommercePipelineExecutionContext>
    {
        private readonly ViewCommander _viewCommander;

        public GetSellableItemStiboAttributeViewBlockForProductOverview(ViewCommander viewCommander)
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

            // Make sure that we target the correct views
            if (string.IsNullOrEmpty(request.ViewName) ||
                !request.ViewName.Equals(catalogViewsPolicy.Master, StringComparison.OrdinalIgnoreCase) &&
                !request.ViewName.Equals(catalogViewsPolicy.Details, StringComparison.OrdinalIgnoreCase) &&
                !request.ViewName.Equals(sellableitemstiboattributesviewspolicy.ProductOverview, StringComparison.OrdinalIgnoreCase) &&
                !isVariationView &&
                !isConnectView)
            {
                return Task.FromResult(arg);
            }

            // Only proceed if the current entity is a sellable item
            if (!(request.Entity is Sitecore.Commerce.Plugin.Catalog.SellableItem))
            {
                return Task.FromResult(arg);
            }

            var sellableItem = (Sitecore.Commerce.Plugin.Catalog.SellableItem)request.Entity;

            // See if we are dealing with the base sellable item or one of its variations.
            var variationId = string.Empty;
            ProductOverview component;
            if (isVariationView && !string.IsNullOrEmpty(arg.ItemId))
            {
                variationId = arg.ItemId;
                component = sellableItem.GetVariation(variationId).GetComponent<ProductOverview>();
            }
            else if (!string.IsNullOrEmpty(arg.ItemId) && arg.VersionedItemId != ("-1"))
            {
                variationId = arg.ItemId;
                component = sellableItem.GetVariation(variationId).GetComponent<ProductOverview>();
            }
            else
            {
                component = sellableItem.GetComponent<ProductOverview>(variationId);
            }

            var targetView = arg;
            bool isEditView = false;

            
            #region 1. Product Overview

            // Check if the edit action was requested
            isEditView = !string.IsNullOrEmpty(arg.Action) && arg.Action.Equals(sellableitemstiboattributesviewspolicy.ProductOverview, StringComparison.OrdinalIgnoreCase);

            if (!isEditView)
            {
                // Create a new view and add it to the current entity view.
                var view = new EntityView
                {
                    Name = context.GetPolicy<SellableItemStiboAttributesViewsPolicy>().ProductOverview,
                    DisplayName = "Product Overview",
                    EntityId = arg.EntityId,
                    DisplayRank = 11,
                    EntityVersion = arg.EntityVersion,
                    ItemId = variationId,
                    Icon = "piece",
                };

                arg.ChildViews.Add(view);
                targetView = view;
            }


            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.UPC_EAN_SAP), component.UPC_EAN_SAP, component.GetDisplayName(nameof(component.UPC_EAN_SAP)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Restricted_States_Regulated), component.Restricted_States_Regulated, component.GetDisplayName(nameof(component.Restricted_States_Regulated)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Color_Code_SAP), component.Color_Code_SAP, component.GetDisplayName(nameof(component.Color_Code_SAP)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Color_Variant), component.Color_Variant, component.GetDisplayName(nameof(component.Color_Variant)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Customer_Exclusive_Material_YN), component.Customer_Exclusive_Material_YN, component.GetDisplayName(nameof(component.Customer_Exclusive_Material_YN)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Grohe_MOT), component.Grohe_MOT, component.GetDisplayName(nameof(component.Grohe_MOT)));

            #endregion
            return Task.FromResult(arg);
        }
    }
}