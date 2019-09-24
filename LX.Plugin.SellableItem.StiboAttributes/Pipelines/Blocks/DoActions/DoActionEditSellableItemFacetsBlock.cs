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
    [PipelineDisplayName(SellableItemStiboAttributesConstants.Pipelines.Blocks.DoActionEditSellableItemSellableItemFacetsBlock)]
    public class DoActionEditSellableItemFacetsBlock : PipelineBlock<EntityView, EntityView, CommercePipelineExecutionContext>
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
        public DoActionEditSellableItemFacetsBlock(CommerceCommander commander)
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


            var sellableItemStiboAttributesViewsPolicy = context.GetPolicy<SellableItemFacetsActionsPolicy>();

            // Only proceed if the right action was invoked
            if (string.IsNullOrEmpty(arg.Action) || !arg.Action.Equals(sellableItemStiboAttributesViewsPolicy.SellableItemFacets, StringComparison.OrdinalIgnoreCase))
            {
                return Task.FromResult(arg);
            }

            // Get the sellable item from the context
            var entity = context.CommerceContext.GetObject<Sitecore.Commerce.Plugin.Catalog.SellableItem>(x => x.Id.Equals(arg.EntityId));
            if (entity == null)
            {
                return Task.FromResult(arg);
            }

            SellableItemFacets component;
            // Get the notes component from the sellable item or its variation
            if (!string.IsNullOrWhiteSpace(arg.ItemId))
            {
                component = entity.GetVariation(arg.ItemId).GetComponent<SellableItemFacets>();
            }
            else
            {
                component = entity.GetComponent<SellableItemFacets>();
            }
            
            component.Item_Shape =
              arg.Properties.FirstOrDefault(x =>
                  x.Name.Equals(nameof(SellableItemFacets.Item_Shape), StringComparison.OrdinalIgnoreCase))?.Value;

            component.Features =
              arg.Properties.FirstOrDefault(x =>
                  x.Name.Equals(nameof(SellableItemFacets.Features), StringComparison.OrdinalIgnoreCase))?.Value;

            component.Certifications =
              arg.Properties.FirstOrDefault(x =>
                  x.Name.Equals(nameof(SellableItemFacets.Certifications), StringComparison.OrdinalIgnoreCase))?.Value;

            component.Water_Type =
              arg.Properties.FirstOrDefault(x =>
                  x.Name.Equals(nameof(SellableItemFacets.Water_Type), StringComparison.OrdinalIgnoreCase))?.Value;

            component.Shower_Type =
              arg.Properties.FirstOrDefault(x =>
                  x.Name.Equals(nameof(SellableItemFacets.Shower_Type), StringComparison.OrdinalIgnoreCase))?.Value;

            component.Dimensions =
              arg.Properties.FirstOrDefault(x =>
                  x.Name.Equals(nameof(SellableItemFacets.Dimensions), StringComparison.OrdinalIgnoreCase))?.Value;

            component.Flow_Rate =
              arg.Properties.FirstOrDefault(x =>
                  x.Name.Equals(nameof(SellableItemFacets.Flow_Rate), StringComparison.OrdinalIgnoreCase))?.Value;

            component.Tub_Dimensions =
              arg.Properties.FirstOrDefault(x =>
                  x.Name.Equals(nameof(SellableItemFacets.Tub_Dimensions), StringComparison.OrdinalIgnoreCase))?.Value;

            component.Vanity_Assembled_Dimensions =
              arg.Properties.FirstOrDefault(x =>
                  x.Name.Equals(nameof(SellableItemFacets.Vanity_Assembled_Dimensions), StringComparison.OrdinalIgnoreCase))?.Value;

            component.Vanity_Cabinet_Dimensions =
              arg.Properties.FirstOrDefault(x =>
                  x.Name.Equals(nameof(SellableItemFacets.Vanity_Cabinet_Dimensions), StringComparison.OrdinalIgnoreCase))?.Value;

            component.Drain_Opening_Size =
              arg.Properties.FirstOrDefault(x =>
                  x.Name.Equals(nameof(SellableItemFacets.Drain_Opening_Size), StringComparison.OrdinalIgnoreCase))?.Value;

            component.Toilet_Seat_Inside_Length =
              arg.Properties.FirstOrDefault(x =>
                  x.Name.Equals(nameof(SellableItemFacets.Toilet_Seat_Inside_Length), StringComparison.OrdinalIgnoreCase))?.Value;

            context.Logger.LogInformation("Current Entity Version : " + entity.Version);

            // Persist changes
            this.Commander.Pipeline<IPersistEntityPipeline>().Run(new PersistEntityArgument(entity), context);

            return Task.FromResult(arg);
        }
    }
}
