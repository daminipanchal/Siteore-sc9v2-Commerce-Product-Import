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

namespace LX.Plugin.SellableItem.StiboAttributes.Pipelines.Blocks.DoActions
{
    [PipelineDisplayName(SellableItemStiboAttributesConstants.Pipelines.Blocks.DoActionEditSellableItemCategoryAttributesBlock)]
    public class DoActionEditSellableItemCategoryAttributesBlock : PipelineBlock<EntityView, EntityView, CommercePipelineExecutionContext>
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
        public DoActionEditSellableItemCategoryAttributesBlock(CommerceCommander commander)
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


            var sellableItemStiboAttributesViewsPolicy = context.GetPolicy<CategoryAttributesActionPolicy>();

            // Only proceed if the right action was invoked
            if (string.IsNullOrEmpty(arg.Action) || !arg.Action.Equals(sellableItemStiboAttributesViewsPolicy.CategoryAttributes, StringComparison.OrdinalIgnoreCase))
            {
                return Task.FromResult(arg);
            }

            // Get the sellable item from the context

            var sellableItemEntity = context.CommerceContext.GetObject<Sitecore.Commerce.Plugin.Catalog.SellableItem>(x => x.Id.Equals(arg.EntityId));
            var categoryEntity = context.CommerceContext.GetObject<Sitecore.Commerce.Plugin.Catalog.Category>(x => x.Id.Equals(arg.EntityId));
            CategoryAttributes component = null;

            // Get the component from the sellable item or its variation
            if (sellableItemEntity != null)
            {
                if (!string.IsNullOrWhiteSpace(arg.ItemId))
                {
                    component = sellableItemEntity.GetVariation(arg.ItemId).GetComponent<CategoryAttributes>();
                }
                else
                {
                    component = sellableItemEntity.GetComponent<CategoryAttributes>();
                }
            }
            if (categoryEntity != null)
            {
                component = categoryEntity.GetComponent<CategoryAttributes>();
            }

            if (component != null)
            {

                component.SitecoreMappingPath =
                    arg.Properties.FirstOrDefault(x =>
                        x.Name.Equals(nameof(CategoryAttributes.SitecoreMappingPath), StringComparison.OrdinalIgnoreCase))?.Value;

                component.ClassificationType =
                    arg.Properties.FirstOrDefault(x =>
                        x.Name.Equals(nameof(CategoryAttributes.ClassificationType), StringComparison.OrdinalIgnoreCase))?.Value;
            }

            if (sellableItemEntity != null)
            {
                context.Logger.LogInformation("Current Entity Version : " + sellableItemEntity.Version);

                // Persist changes
                this.Commander.Pipeline<IPersistEntityPipeline>().Run(new PersistEntityArgument(sellableItemEntity), context);
            }
            if (categoryEntity != null)
            {
                context.Logger.LogInformation("Current Entity Version : " + categoryEntity.Version);

                // Persist changes
                this.Commander.Pipeline<IPersistEntityPipeline>().Run(new PersistEntityArgument(categoryEntity), context);
            }

            return Task.FromResult(arg);
        }
    }
}