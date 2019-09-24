
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
    [PipelineDisplayName(SellableItemStiboAttributesConstants.Pipelines.Blocks.DoActionEditSellableItemInstallationAndInstructionsBlock)]
    public class DoActionEditSellableItemInstallationAndInstructionsBlock : PipelineBlock<EntityView, EntityView, CommercePipelineExecutionContext>
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
        public DoActionEditSellableItemInstallationAndInstructionsBlock(CommerceCommander commander)
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


            var sellableItemStiboAttributesViewsPolicy = context.GetPolicy<InstallationAndInstructionsActionsPolicy>();

            // Only proceed if the right action was invoked
            if (string.IsNullOrEmpty(arg.Action) || !arg.Action.Equals(sellableItemStiboAttributesViewsPolicy.InstallationAndInstructions, StringComparison.OrdinalIgnoreCase))
            {
                return Task.FromResult(arg);
            }

            // Get the sellable item from the context
            var entity = context.CommerceContext.GetObject<Sitecore.Commerce.Plugin.Catalog.SellableItem>(x => x.Id.Equals(arg.EntityId));
            if (entity == null)
            {
                return Task.FromResult(arg);
            }

            InstallationAndInstructions component;
            // Get the notes component from the sellable item or its variation
            if (!string.IsNullOrWhiteSpace(arg.ItemId))
            {
                component = entity.GetVariation(arg.ItemId).GetComponent<InstallationAndInstructions>();
            }
            else
            {
                component = entity.GetComponent<InstallationAndInstructions>();
            }

            component.Installation_Instruction_URL =
                arg.Properties.FirstOrDefault(x =>
                    x.Name.Equals(nameof(InstallationAndInstructions.Installation_Instruction_URL), StringComparison.OrdinalIgnoreCase))?.Value;

            component.Kitchen_Sink_Mount_Location =
                arg.Properties.FirstOrDefault(x =>
                    x.Name.Equals(nameof(InstallationAndInstructions.Kitchen_Sink_Mount_Location), StringComparison.OrdinalIgnoreCase))?.Value;


            component.Warranty_PDF_URL =
                arg.Properties.FirstOrDefault(x =>
                    x.Name.Equals(nameof(InstallationAndInstructions.Warranty_PDF_URL), StringComparison.OrdinalIgnoreCase))?.Value;

            component.Dimensional_Drawing_URL =
                arg.Properties.FirstOrDefault(x =>
                    x.Name.Equals(nameof(InstallationAndInstructions.Dimensional_Drawing_URL), StringComparison.OrdinalIgnoreCase))?.Value;

            component.EPD_URL =
                arg.Properties.FirstOrDefault(x =>
                    x.Name.Equals(nameof(InstallationAndInstructions.EPD_URL), StringComparison.OrdinalIgnoreCase))?.Value;

            component.User_Instr_PDF_URL =
                arg.Properties.FirstOrDefault(x =>
                    x.Name.Equals(nameof(InstallationAndInstructions.User_Instr_PDF_URL), StringComparison.OrdinalIgnoreCase))?.Value;

            component.Use_and_Care_URL =
                arg.Properties.FirstOrDefault(x =>
                    x.Name.Equals(nameof(InstallationAndInstructions.Use_and_Care_URL), StringComparison.OrdinalIgnoreCase))?.Value;

            component.MSDS_URL =
                arg.Properties.FirstOrDefault(x =>
                    x.Name.Equals(nameof(InstallationAndInstructions.MSDS_URL), StringComparison.OrdinalIgnoreCase))?.Value;

            context.Logger.LogInformation("Current Entity Version : " + entity.Version);

            // Persist changes
            this.Commander.Pipeline<IPersistEntityPipeline>().Run(new PersistEntityArgument(entity), context);

            return Task.FromResult(arg);
        }
    }
}
