using Sitecore.Commerce.Core;
using Sitecore.Commerce.EntityViews;
using Sitecore.Framework.Conditions;
using Sitecore.Framework.Pipelines;
using System;
using System.Linq;
using System.Threading.Tasks;
using Sitecore.Commerce.Plugin.Catalog;
using LX.Plugin.SellableItem.StiboAttributes.Policies;
using Microsoft.Extensions.Logging;
using LX.Plugin.SellableItem.StiboAttributes.Components;

namespace LX.Plugin.SellableItem.StiboAttributes.Pipelines.Blocks
{
    [PipelineDisplayName(SellableItemStiboAttributesConstants.Pipelines.Blocks.DoActionEditSellableItemAdditionalResourcesBlock)]
    public class DoActionEditSellableItemAdditionalResourcesBlock : PipelineBlock<EntityView, EntityView, CommercePipelineExecutionContext>
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
        public DoActionEditSellableItemAdditionalResourcesBlock(CommerceCommander commander)
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


            var sellableItemStiboAttributesViewsPolicy = context.GetPolicy<AdditionalResourcesActionsPolicy>();

            // Only proceed if the right action was invoked
            if (string.IsNullOrEmpty(arg.Action) || !arg.Action.Equals(sellableItemStiboAttributesViewsPolicy.AdditionalResources, StringComparison.OrdinalIgnoreCase))
            {
                return Task.FromResult(arg);
            }

            // Get the sellable item from the context
            var entity = context.CommerceContext.GetObject<Sitecore.Commerce.Plugin.Catalog.SellableItem>(x => x.Id.Equals(arg.EntityId));
            if (entity == null)
            {
                return Task.FromResult(arg);
            }
            AdditionalResources component;
            // Get the component from the sellable item or its variation
            if (!string.IsNullOrWhiteSpace(arg.ItemId))
            {
                component = entity.GetVariation(arg.ItemId).GetComponent<AdditionalResources>();
            }
            else
            {
                component = entity.GetComponent<AdditionalResources>();
            }

            component.CAD_2D_URL =
            arg.Properties.FirstOrDefault(x =>
                x.Name.Equals(nameof(AdditionalResources.CAD_2D_URL), StringComparison.OrdinalIgnoreCase))?.Value;

            component.CAD_3D_URL =
                arg.Properties.FirstOrDefault(x =>
                    x.Name.Equals(nameof(AdditionalResources.CAD_3D_URL), StringComparison.OrdinalIgnoreCase))?.Value;

            component.Spec_Sheet_URL =
                arg.Properties.FirstOrDefault(x =>
                    x.Name.Equals(nameof(AdditionalResources.Spec_Sheet_URL), StringComparison.OrdinalIgnoreCase))?.Value;


            context.Logger.LogInformation("Current Entity Version : " + entity.Version);

            // Persist changes
            this.Commander.Pipeline<IPersistEntityPipeline>().Run(new PersistEntityArgument(entity), context);

            return Task.FromResult(arg);
        }
    }
}
