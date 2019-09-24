
using Sitecore.Commerce.Core;
using Sitecore.Commerce.EntityViews;
using Sitecore.Framework.Conditions;
using Sitecore.Framework.Pipelines;
using System;
using System.Linq;
using System.Threading.Tasks;
using Sitecore.Commerce.Plugin.Catalog;
using LX.Plugin.SellableItem.StiboAttributes.Components;
using LX.Plugin.SellableItem.StiboAttributes.Policies;
using Microsoft.Extensions.Logging;

namespace LX.Plugin.SellableItem.StiboAttributes.Pipelines.Blocks
{
    [PipelineDisplayName(SellableItemStiboAttributesConstants.Pipelines.Blocks.DoActionEditSellableItemOtherSpecificationsBlock)]
    public class DoActionEditSellableItemOtherSpecificationsBlock : PipelineBlock<EntityView, EntityView, CommercePipelineExecutionContext>
    {
        /// <summary>
        /// Gets or sets the commander.
        /// </summary>
        /// <value>
        /// The commander.
        /// </value>
        protected CommerceCommander Commander { get; set; }

        /// <inheritdoc />
        /// <summary>Initializes a new instance of the <see cref="T:Sitecore.Framework.Pipelines.PipelineBlock" /> class.</summary>
        /// <param name="commander">The commerce commander.</param>
        public DoActionEditSellableItemOtherSpecificationsBlock(CommerceCommander commander)
            : base(null)
        {

            this.Commander = commander;

        }

        /// <summary>
        /// The execute.
        /// </summary>
        /// <param name="arg">
        /// The pipeline argument.
        /// </param>
        /// <param name="context">
        /// The context.
        /// </param>
        /// <returns>
        /// The <see cref="PipelineArgument"/>.
        /// </returns>
        public override Task<EntityView> Run(EntityView arg, CommercePipelineExecutionContext context)
        {
            Condition.Requires(arg).IsNotNull($"{Name}: The argument cannot be null.");


            var sellableItemStiboAttributesViewsPolicy = context.GetPolicy<OtherSpecificationsActionsPolicy>();

            // Only proceed if the right action was invoked
            if (string.IsNullOrEmpty(arg.Action) || !arg.Action.Equals(sellableItemStiboAttributesViewsPolicy.OtherSpecifications, StringComparison.OrdinalIgnoreCase))
            {
                return Task.FromResult(arg);
            }

            // Get the sellable item from the context
            var entity = context.CommerceContext.GetObject<Sitecore.Commerce.Plugin.Catalog.SellableItem>(x => x.Id.Equals(arg.EntityId));
            if (entity == null)
            {
                return Task.FromResult(arg);
            }

            OtherSpecifications component;
            // Get the notes component from the sellable item or its variation
            if (!string.IsNullOrWhiteSpace(arg.ItemId))
            {
                component = entity.GetVariation(arg.ItemId).GetComponent<OtherSpecifications>();
            }
            else
            {
                component = entity.GetComponent<OtherSpecifications>();
            }

            int tempIntValue = 0;
            decimal tempDoubleValue = 0;

            ViewProperty viewProperty = arg.Properties.FirstOrDefault(x => x.Name.Equals(nameof(OtherSpecifications.Each_Weight_Gross_Weight_SAP), StringComparison.OrdinalIgnoreCase));
            if (!string.IsNullOrEmpty(viewProperty?.Value) && decimal.TryParse(viewProperty.Value, out tempDoubleValue))
                component.Each_Weight_Gross_Weight_SAP = tempDoubleValue;

            component.Warranty_Type =
                arg.Properties.FirstOrDefault(x =>
                    x.Name.Equals(nameof(OtherSpecifications.Warranty_Type), StringComparison.OrdinalIgnoreCase))?.Value;

            component.UN_NA_Number =
                arg.Properties.FirstOrDefault(x =>
                    x.Name.Equals(nameof(OtherSpecifications.UN_NA_Number), StringComparison.OrdinalIgnoreCase))?.Value;


            component.UNSPSC_Code_United_Nations_Code =
                arg.Properties.FirstOrDefault(x =>
                    x.Name.Equals(nameof(OtherSpecifications.UNSPSC_Code_United_Nations_Code), StringComparison.OrdinalIgnoreCase))?.Value;

            component.NFPA_Storage_Classification =
                arg.Properties.FirstOrDefault(x =>
                    x.Name.Equals(nameof(OtherSpecifications.NFPA_Storage_Classification), StringComparison.OrdinalIgnoreCase))?.Value;

            component.ORMD_YN =
                arg.Properties.FirstOrDefault(x =>
                    x.Name.Equals(nameof(OtherSpecifications.ORMD_YN), StringComparison.OrdinalIgnoreCase))?.Value;

            component.Hazmat_YN =
                arg.Properties.FirstOrDefault(x =>
                    x.Name.Equals(nameof(OtherSpecifications.Hazmat_YN), StringComparison.OrdinalIgnoreCase))?.Value;

            component.Hazard_Class_Number =
                arg.Properties.FirstOrDefault(x =>
                    x.Name.Equals(nameof(OtherSpecifications.Hazard_Class_Number), StringComparison.OrdinalIgnoreCase))?.Value;

            component.Kit_SMO_POD =
                arg.Properties.FirstOrDefault(x =>
                    x.Name.Equals(nameof(OtherSpecifications.Kit_SMO_POD), StringComparison.OrdinalIgnoreCase))?.Value;

            component.Mfg_Part_Number_SAP =
                arg.Properties.FirstOrDefault(x =>
                    x.Name.Equals(nameof(OtherSpecifications.Mfg_Part_Number_SAP), StringComparison.OrdinalIgnoreCase))?.Value;

            component.Kitchen_Bath =
                arg.Properties.FirstOrDefault(x =>
                    x.Name.Equals(nameof(OtherSpecifications.Kitchen_Bath), StringComparison.OrdinalIgnoreCase))?.Value;


            ViewProperty viewPropertyRetailPrice = arg.Properties.FirstOrDefault(x => x.Name.Equals(nameof(OtherSpecifications.Retail_Price), StringComparison.OrdinalIgnoreCase));
            if (!string.IsNullOrEmpty(viewPropertyRetailPrice?.Value) && decimal.TryParse(viewPropertyRetailPrice.Value, out tempDoubleValue))
                component.Retail_Price = tempDoubleValue;

            ViewProperty viewPropertyLength = arg.Properties.FirstOrDefault(x => x.Name.Equals(nameof(OtherSpecifications.Bowl_Front_To_Back_Length), StringComparison.OrdinalIgnoreCase));
            if (!string.IsNullOrEmpty(viewPropertyLength?.Value) && int.TryParse(viewPropertyLength.Value, out tempIntValue))
                component.Bowl_Front_To_Back_Length = tempIntValue;

            component.Residential_Or_Commercial =
                arg.Properties.FirstOrDefault(x =>
                    x.Name.Equals(nameof(OtherSpecifications.Residential_Or_Commercial), StringComparison.OrdinalIgnoreCase))?.Value;

            component.Battery_Lifetime_Cycles =
                arg.Properties.FirstOrDefault(x =>
                    x.Name.Equals(nameof(OtherSpecifications.Battery_Lifetime_Cycles), StringComparison.OrdinalIgnoreCase))?.Value;

            component.Batteries_Required =
                arg.Properties.FirstOrDefault(x =>
                    x.Name.Equals(nameof(OtherSpecifications.Batteries_Required), StringComparison.OrdinalIgnoreCase))?.Value;

            component.Included_In_Box =
                arg.Properties.FirstOrDefault(x =>
                    x.Name.Equals(nameof(OtherSpecifications.Included_In_Box), StringComparison.OrdinalIgnoreCase))?.Value;

            component.Freestanding_Tub_Style =
                arg.Properties.FirstOrDefault(x =>
                    x.Name.Equals(nameof(OtherSpecifications.Freestanding_Tub_Style), StringComparison.OrdinalIgnoreCase))?.Value;

            component.Faucet_Included_YN =
                arg.Properties.FirstOrDefault(x =>
                    x.Name.Equals(nameof(OtherSpecifications.Faucet_Included_YN), StringComparison.OrdinalIgnoreCase))?.Value;

            component.Removable_YN =
                arg.Properties.FirstOrDefault(x =>
                    x.Name.Equals(nameof(OtherSpecifications.Removable_YN), StringComparison.OrdinalIgnoreCase))?.Value;

            bool tempBoolValue = false;
            ViewProperty viewPropertyIsSellable = arg.Properties.FirstOrDefault(x => x.Name.Equals(nameof(OtherSpecifications.Is_Sellable), StringComparison.OrdinalIgnoreCase));
            if (!string.IsNullOrEmpty(viewPropertyIsSellable?.Value) && bool.TryParse(viewPropertyIsSellable.Value, out tempBoolValue))
                component.Is_Sellable = tempBoolValue;

            context.Logger.LogInformation("Current Entity Version : " + entity.Version);

            // Persist changes
            this.Commander.Pipeline<IPersistEntityPipeline>().Run(new PersistEntityArgument(entity), context);

            return Task.FromResult(arg);
        }
    }
}
