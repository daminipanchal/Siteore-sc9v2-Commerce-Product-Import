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
    [PipelineDisplayName(SellableItemStiboAttributesConstants.Pipelines.Blocks.GetSellableItemStiboAttributeViewBlockForGlobalStiboAttributes)]
    public class GetSellableItemStiboAttributeViewBlockForGlobalStiboAttributes : PipelineBlock<EntityView, EntityView, CommercePipelineExecutionContext>
    {
        private readonly ViewCommander _viewCommander;

        public GetSellableItemStiboAttributeViewBlockForGlobalStiboAttributes(ViewCommander viewCommander)
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
                !request.ViewName.Equals(sellableitemstiboattributesviewspolicy.GlobalStiboAttributes, StringComparison.OrdinalIgnoreCase) &&
                !isVariationView &&
                !isConnectView)
            {
                return Task.FromResult(arg);
            }

            var sellableItem = (Sitecore.Commerce.Plugin.Catalog.SellableItem)request.Entity;
            GlobalStiboAttributes component;
            // See if we are dealing with the base sellable item or one of its variations.
            var variationId = string.Empty;
            if (isVariationView && !string.IsNullOrEmpty(arg.ItemId))
            {
                variationId = arg.ItemId;
                component = sellableItem.GetVariation(variationId).GetComponent<GlobalStiboAttributes>();
            }
            else if (!string.IsNullOrEmpty(arg.ItemId) && arg.VersionedItemId != ("-1"))
            {
                variationId = arg.ItemId;
                component = sellableItem.GetVariation(variationId).GetComponent<GlobalStiboAttributes>();
            }
            else
            {
                component = sellableItem.GetComponent<GlobalStiboAttributes>(variationId);
            }

            var targetView = arg;
            bool isEditView = false;

            #region 16. Global Stibo Attributes
            isEditView = !string.IsNullOrEmpty(arg.Action) && arg.Action.Equals(sellableitemstiboattributesviewspolicy.GlobalStiboAttributes, StringComparison.OrdinalIgnoreCase);
            if (!isEditView)
            {
                // Create a new view and add it to the current entity view.
                var view = new EntityView
                {
                    Name = context.GetPolicy<SellableItemStiboAttributesViewsPolicy>().GlobalStiboAttributes,
                    DisplayName = "Global Stibo Attributes",
                    EntityId = arg.EntityId,
                    DisplayRank = 5,
                    EntityVersion = arg.EntityVersion,
                    ItemId = variationId,
                    Icon = "piece",
                };

                arg.ChildViews.Add(view);
                targetView = view;
            }

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Is_Discontinued), component.Is_Discontinued, component.GetDisplayName(nameof(component.Is_Discontinued)));


            AddSellableItemProperties.AddPropertiesToViewWithSection (targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Base_UOM_SAP), component.Base_UOM_SAP, component.GetDisplayName(nameof(component.Base_UOM_SAP)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.AltUOMConvFac), component.AltUOMConvFac, component.GetDisplayName(nameof(component.AltUOMConvFac)));


            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.SKU_Status), component.SKU_Status, component.GetDisplayName(nameof(component.SKU_Status)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Product_Status), component.Product_Status, component.GetDisplayName(nameof(component.Product_Status)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.SAP_SKU_Status_SAP), component.SAP_SKU_Status_SAP, component.GetDisplayName(nameof(component.SAP_SKU_Status_SAP)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Stock_Level_SAP), component.Stock_Level_SAP, component.GetDisplayName(nameof(component.Stock_Level_SAP)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Small_Parcel_Postable_SAP_YN), component.Small_Parcel_Postable_SAP_YN, component.GetDisplayName(nameof(component.Small_Parcel_Postable_SAP_YN)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.SAPLink), component.SAPLink, component.GetDisplayName(nameof(component.SAPLink)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.DC_Location), component.DC_Location, component.GetDisplayName(nameof(component.DC_Location)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.MTO_or_MTS), component.MTO_or_MTS, component.GetDisplayName(nameof(component.MTO_or_MTS)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.ParentNumber), component.ParentNumber, component.GetDisplayName(nameof(component.ParentNumber)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Pkg_Component_1), component.Pkg_Component_1, component.GetDisplayName(nameof(component.Pkg_Component_1)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Shipping_Plants_SAP), component.Shipping_Plants_SAP, component.GetDisplayName(nameof(component.Shipping_Plants_SAP)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Shower_No_Of_Showerheads), component.Shower_No_Of_Showerheads, component.GetDisplayName(nameof(component.Shower_No_Of_Showerheads)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Hand_Shower_Included_YN), component.Hand_Shower_Included_YN, component.GetDisplayName(nameof(component.Hand_Shower_Included_YN)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Bath_Accessory_Application), component.Bath_Accessory_Application, component.GetDisplayName(nameof(component.Bath_Accessory_Application)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Sink_No_Of_Pieces), component.Sink_No_Of_Pieces, component.GetDisplayName(nameof(component.Sink_No_Of_Pieces)));
            #endregion


            return Task.FromResult(arg);
        }
    }
}