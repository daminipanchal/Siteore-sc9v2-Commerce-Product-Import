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
    [PipelineDisplayName(SellableItemStiboAttributesConstants.Pipelines.Blocks.DoActionEditSellableItemStiboAttributesBlock)]
    public class DoActionEditSellableItemGlobalStiboAttributesBlock : PipelineBlock<EntityView, EntityView, CommercePipelineExecutionContext>
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
        public DoActionEditSellableItemGlobalStiboAttributesBlock(CommerceCommander commander)
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

            var sellableItemStiboAttributesViewsPolicy = context.GetPolicy<GlobalStiboAttributesActionPolicy>();

            // Only proceed if the right action was invoked
            if (string.IsNullOrEmpty(arg.Action)
                || !arg.Action.Equals(sellableItemStiboAttributesViewsPolicy.GlobalStiboAttributes, StringComparison.OrdinalIgnoreCase)
                )
            {
                return Task.FromResult(arg);
            }


            // Get the sellable item from the context
            var entity = context.CommerceContext.GetObject<Sitecore.Commerce.Plugin.Catalog.SellableItem>(x => x.Id.Equals(arg.EntityId));
            if (entity == null)
            {
                return Task.FromResult(arg);
            }

            GlobalStiboAttributes component;
            // Get the notes component from the sellable item or its variation
            if (!string.IsNullOrWhiteSpace(arg.ItemId))
            {
                component = entity.GetVariation(arg.ItemId).GetComponent<GlobalStiboAttributes>();
            }
            else
            {
                component = entity.GetComponent<GlobalStiboAttributes>();
            }

            // Map entity view properties to component
            int tempIntValue = 0;

            component.Base_UOM_SAP =
                arg.Properties.FirstOrDefault(x =>
                    x.Name.Equals(nameof(GlobalStiboAttributes.Base_UOM_SAP), StringComparison.OrdinalIgnoreCase))?.Value;

            ViewProperty viewProperty = arg.Properties.FirstOrDefault(x => x.Name.Equals(nameof(GlobalStiboAttributes.AltUOMConvFac), StringComparison.OrdinalIgnoreCase));
            if (!string.IsNullOrEmpty(viewProperty?.Value) && int.TryParse(viewProperty.Value, out tempIntValue))
                component.AltUOMConvFac = tempIntValue;

            bool tempBoolValue = false;
            ViewProperty viewPropertyDiscontinued = arg.Properties.FirstOrDefault(x => x.Name.Equals(nameof(GlobalStiboAttributes.Is_Discontinued), StringComparison.OrdinalIgnoreCase));
            if (!string.IsNullOrEmpty(viewPropertyDiscontinued?.Value) && bool.TryParse(viewPropertyDiscontinued.Value, out tempBoolValue))
                component.Is_Discontinued = tempBoolValue;

            component.SKU_Status =
                arg.Properties.FirstOrDefault(x =>
                    x.Name.Equals(nameof(GlobalStiboAttributes.SKU_Status), StringComparison.OrdinalIgnoreCase))?.Value;

            component.Product_Status =
               arg.Properties.FirstOrDefault(x =>
                   x.Name.Equals(nameof(GlobalStiboAttributes.Product_Status), StringComparison.OrdinalIgnoreCase))?.Value;

            component.SAP_SKU_Status_SAP =
               arg.Properties.FirstOrDefault(x =>
                   x.Name.Equals(nameof(GlobalStiboAttributes.SAP_SKU_Status_SAP), StringComparison.OrdinalIgnoreCase))?.Value;

            ViewProperty viewPropertyStock_Level_SAP = arg.Properties.FirstOrDefault(x => x.Name.Equals(nameof(GlobalStiboAttributes.Stock_Level_SAP), StringComparison.OrdinalIgnoreCase));
            if (!string.IsNullOrEmpty(viewPropertyStock_Level_SAP?.Value) && int.TryParse(viewPropertyStock_Level_SAP.Value, out tempIntValue))
                component.Stock_Level_SAP = tempIntValue;

            component.Small_Parcel_Postable_SAP_YN =
         arg.Properties.FirstOrDefault(x =>
             x.Name.Equals(nameof(GlobalStiboAttributes.Small_Parcel_Postable_SAP_YN), StringComparison.OrdinalIgnoreCase))?.Value;

            component.SAPLink =
                arg.Properties.FirstOrDefault(x =>
                    x.Name.Equals(nameof(GlobalStiboAttributes.SAPLink), StringComparison.OrdinalIgnoreCase))?.Value;

            component.DC_Location =
                arg.Properties.FirstOrDefault(x =>
                    x.Name.Equals(nameof(GlobalStiboAttributes.DC_Location), StringComparison.OrdinalIgnoreCase))?.Value;

            component.MTO_or_MTS =
                arg.Properties.FirstOrDefault(x =>
                    x.Name.Equals(nameof(GlobalStiboAttributes.MTO_or_MTS), StringComparison.OrdinalIgnoreCase))?.Value;

            component.Pkg_Component_1 =
                arg.Properties.FirstOrDefault(x =>
                    x.Name.Equals(nameof(GlobalStiboAttributes.Pkg_Component_1), StringComparison.OrdinalIgnoreCase))?.Value;

            component.ParentNumber =
                arg.Properties.FirstOrDefault(x =>
                    x.Name.Equals(nameof(GlobalStiboAttributes.ParentNumber), StringComparison.OrdinalIgnoreCase))?.Value;

            component.Shipping_Plants_SAP =
                arg.Properties.FirstOrDefault(x =>
                    x.Name.Equals(nameof(GlobalStiboAttributes.Shipping_Plants_SAP), StringComparison.OrdinalIgnoreCase))?.Value;

            ViewProperty viewPropertyShower_No_Of_Showerheads = arg.Properties.FirstOrDefault(x => x.Name.Equals(nameof(GlobalStiboAttributes.Shower_No_Of_Showerheads), StringComparison.OrdinalIgnoreCase));
            if (!string.IsNullOrEmpty(viewPropertyShower_No_Of_Showerheads?.Value) && int.TryParse(viewPropertyShower_No_Of_Showerheads.Value, out tempIntValue))
                component.Shower_No_Of_Showerheads = tempIntValue;

            component.Hand_Shower_Included_YN = arg.Properties.FirstOrDefault(x =>
            x.Name.Equals(nameof(GlobalStiboAttributes.Hand_Shower_Included_YN), StringComparison.OrdinalIgnoreCase))?.Value;

            component.Bath_Accessory_Application =
                arg.Properties.FirstOrDefault(x =>
                    x.Name.Equals(nameof(GlobalStiboAttributes.Bath_Accessory_Application), StringComparison.OrdinalIgnoreCase))?.Value;

            ViewProperty viewPropertySink_No_Of_Pieces = arg.Properties.FirstOrDefault(x => x.Name.Equals(nameof(GlobalStiboAttributes.Sink_No_Of_Pieces), StringComparison.OrdinalIgnoreCase));
            if (!string.IsNullOrEmpty(viewPropertyShower_No_Of_Showerheads?.Value) && int.TryParse(viewPropertySink_No_Of_Pieces.Value, out tempIntValue))
                component.Sink_No_Of_Pieces = tempIntValue;

            context.Logger.LogInformation("Current Entity Version : " + entity.Version);

            // Persist changes
            this.Commander.Pipeline<IPersistEntityPipeline>().Run(new PersistEntityArgument(entity), context);

            return Task.FromResult(arg);
        }
    }
}
