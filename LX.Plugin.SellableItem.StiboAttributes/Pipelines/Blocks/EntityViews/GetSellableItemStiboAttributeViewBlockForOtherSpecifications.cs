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
    [PipelineDisplayName(SellableItemStiboAttributesConstants.Pipelines.Blocks.GetSellableItemStiboAttributeViewBlockForOtherSpecifications)]
    public class GetSellableItemStiboAttributeViewBlockForOtherSpecifications : PipelineBlock<EntityView, EntityView, CommercePipelineExecutionContext>
    {
        private readonly ViewCommander _viewCommander;

        public GetSellableItemStiboAttributeViewBlockForOtherSpecifications(ViewCommander viewCommander)
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
                     !request.ViewName.Equals(sellableitemstiboattributesviewspolicy.OtherSpecifications, StringComparison.OrdinalIgnoreCase) && !isVariationView && !isConnectView)
            {
                return Task.FromResult(arg);
            }
            var sellableItem = (Sitecore.Commerce.Plugin.Catalog.SellableItem)request.Entity;

            // See if we are dealing with the base sellable item or one of its variations.
            var variationId = string.Empty;
            OtherSpecifications component;
            if (isVariationView && !string.IsNullOrEmpty(arg.ItemId))
            {
                variationId = arg.ItemId;
                component = sellableItem.GetVariation(variationId).GetComponent<OtherSpecifications>();
            }
            else if (!string.IsNullOrEmpty(arg.ItemId) && arg.VersionedItemId != ("-1"))
            {
                variationId = arg.ItemId;
                component = sellableItem.GetVariation(variationId).GetComponent<OtherSpecifications>();
            }
            else
            {
                component = sellableItem.GetComponent<OtherSpecifications>(variationId);
            }


            var targetView = arg;
            bool isEditView = false;


            #region 15. Other Specifications
            isEditView = !string.IsNullOrEmpty(arg.Action) && arg.Action.Equals(sellableitemstiboattributesviewspolicy.OtherSpecifications, StringComparison.OrdinalIgnoreCase);
            if (!isEditView)
            {
                // Create a new view and add it to the current entity view.
                var view = new EntityView
                {
                    Name = context.GetPolicy<SellableItemStiboAttributesViewsPolicy>().OtherSpecifications,
                    DisplayName = "Other Specifications",
                    EntityId = arg.EntityId,
                    DisplayRank = 8,
                    EntityVersion = arg.EntityVersion,
                    ItemId = variationId,
                    Icon = "piece",
                };

                arg.ChildViews.Add(view);
                targetView = view;
            }

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Each_Weight_Gross_Weight_SAP), component.Each_Weight_Gross_Weight_SAP, component.GetDisplayName(nameof(component.Each_Weight_Gross_Weight_SAP)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Warranty_Type), component.Warranty_Type, component.GetDisplayName(nameof(component.Warranty_Type)));

            //AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.DangerousGoodsRegulation), component.DangerousGoodsRegulation, component.GetDisplayName(nameof(component.DangerousGoodsRegulation)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.UN_NA_Number), component.UN_NA_Number, component.GetDisplayName(nameof(component.UN_NA_Number)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.UNSPSC_Code_United_Nations_Code), component.UNSPSC_Code_United_Nations_Code, component.GetDisplayName(nameof(component.UNSPSC_Code_United_Nations_Code)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.NFPA_Storage_Classification), component.NFPA_Storage_Classification, component.GetDisplayName(nameof(component.NFPA_Storage_Classification)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.ORMD_YN), component.ORMD_YN, component.GetDisplayName(nameof(component.ORMD_YN)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Hazmat_YN), component.Hazmat_YN, component.GetDisplayName(nameof(component.Hazmat_YN)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Hazard_Class_Number), component.Hazard_Class_Number, component.GetDisplayName(nameof(component.Hazard_Class_Number)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Kit_SMO_POD), component.Kit_SMO_POD, component.GetDisplayName(nameof(component.Kit_SMO_POD)));


            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Mfg_Part_Number_SAP), component.Mfg_Part_Number_SAP, component.GetDisplayName(nameof(component.Mfg_Part_Number_SAP)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Kitchen_Bath), component.Kitchen_Bath, component.GetDisplayName(nameof(component.Kitchen_Bath)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Retail_Price), component.Retail_Price, component.GetDisplayName(nameof(component.Retail_Price)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Bowl_Front_To_Back_Length), component.Bowl_Front_To_Back_Length, component.GetDisplayName(nameof(component.Bowl_Front_To_Back_Length)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Residential_Or_Commercial), component.Residential_Or_Commercial, component.GetDisplayName(nameof(component.Residential_Or_Commercial)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Battery_Lifetime_Cycles), component.Battery_Lifetime_Cycles, component.GetDisplayName(nameof(component.Battery_Lifetime_Cycles)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Batteries_Required), component.Batteries_Required, component.GetDisplayName(nameof(component.Batteries_Required)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Included_In_Box), component.Included_In_Box, component.GetDisplayName(nameof(component.Included_In_Box)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Freestanding_Tub_Style), component.Freestanding_Tub_Style, component.GetDisplayName(nameof(component.Freestanding_Tub_Style)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Faucet_Included_YN), component.Faucet_Included_YN, component.GetDisplayName(nameof(component.Faucet_Included_YN)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Removable_YN), component.Removable_YN, component.GetDisplayName(nameof(component.Removable_YN)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Is_Sellable), component.Is_Sellable, component.GetDisplayName(nameof(component.Is_Sellable)));
            #endregion


            return Task.FromResult(arg);
        }
    }
}