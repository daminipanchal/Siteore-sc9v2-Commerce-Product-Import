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
    [PipelineDisplayName(SellableItemStiboAttributesConstants.Pipelines.Blocks.DoActionEditSellableItemProductFeaturesBlock)]
    public class DoActionEditSellableItemProductFeaturesBlock : PipelineBlock<EntityView, EntityView, CommercePipelineExecutionContext>
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
        public DoActionEditSellableItemProductFeaturesBlock(CommerceCommander commander)
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


            var productFeatureActionPolicy = context.GetPolicy<ProductFeaturesActionsPolicy>();

            // Only proceed if the right action was invoked
            if (string.IsNullOrEmpty(arg.Action) || !arg.Action.Equals(productFeatureActionPolicy.ProductFeatures, StringComparison.OrdinalIgnoreCase))
            {
                return Task.FromResult(arg);
            }

            // Get the sellable item from the context
            var entity = context.CommerceContext.GetObject<Sitecore.Commerce.Plugin.Catalog.SellableItem>(x => x.Id.Equals(arg.EntityId));
            if (entity == null)
            {
                return Task.FromResult(arg);
            }

            ProductFeatures component = null;
            // Get the notes component from the sellable item or its variation
            if (!string.IsNullOrWhiteSpace(arg.ItemId))
            {
                component = entity.GetVariation(arg.ItemId).GetComponent<ProductFeatures>();
            }
            else
            {
                component = entity.GetComponent<ProductFeatures>();
            }


            component.Feature_Bullets_1 =
                arg.Properties.FirstOrDefault(x =>
                    x.Name.Equals(nameof(ProductFeatures.Feature_Bullets_1), StringComparison.OrdinalIgnoreCase))?.Value;

            component.Feature_Bullets_2 =
               arg.Properties.FirstOrDefault(x =>
                   x.Name.Equals(nameof(ProductFeatures.Feature_Bullets_2), StringComparison.OrdinalIgnoreCase))?.Value;

            component.Feature_Bullets_3 =
               arg.Properties.FirstOrDefault(x =>
                   x.Name.Equals(nameof(ProductFeatures.Feature_Bullets_3), StringComparison.OrdinalIgnoreCase))?.Value;

            component.Feature_Bullets_4 =
               arg.Properties.FirstOrDefault(x =>
                   x.Name.Equals(nameof(ProductFeatures.Feature_Bullets_4), StringComparison.OrdinalIgnoreCase))?.Value;

            component.Feature_Bullets_5 =
               arg.Properties.FirstOrDefault(x =>
                   x.Name.Equals(nameof(ProductFeatures.Feature_Bullets_5), StringComparison.OrdinalIgnoreCase))?.Value;

            component.Feature_Bullets_6 =
               arg.Properties.FirstOrDefault(x =>
                   x.Name.Equals(nameof(ProductFeatures.Feature_Bullets_6), StringComparison.OrdinalIgnoreCase))?.Value;

            component.Feature_Bullets_7 =
               arg.Properties.FirstOrDefault(x =>
                   x.Name.Equals(nameof(ProductFeatures.Feature_Bullets_7), StringComparison.OrdinalIgnoreCase))?.Value;

            component.Feature_Bullets_8 =
               arg.Properties.FirstOrDefault(x =>
                   x.Name.Equals(nameof(ProductFeatures.Feature_Bullets_8), StringComparison.OrdinalIgnoreCase))?.Value;

            component.Feature_Bullets_9 =
               arg.Properties.FirstOrDefault(x =>
                   x.Name.Equals(nameof(ProductFeatures.Feature_Bullets_9), StringComparison.OrdinalIgnoreCase))?.Value;

            component.Feature_Bullets_10 =
               arg.Properties.FirstOrDefault(x =>
                   x.Name.Equals(nameof(ProductFeatures.Feature_Bullets_10), StringComparison.OrdinalIgnoreCase))?.Value;

            component.Marketing_Claims_1 =
              arg.Properties.FirstOrDefault(x =>
                  x.Name.Equals(nameof(ProductFeatures.Marketing_Claims_1), StringComparison.OrdinalIgnoreCase))?.Value;

            component.Fitting_Features =
           arg.Properties.FirstOrDefault(x =>
               x.Name.Equals(nameof(ProductFeatures.Fitting_Features), StringComparison.OrdinalIgnoreCase))?.Value;

            component.Mirror_Bath_Product_Type =
                 arg.Properties.FirstOrDefault(x =>
                     x.Name.Equals(nameof(ProductFeatures.Mirror_Bath_Product_Type), StringComparison.OrdinalIgnoreCase))?.Value;

            component.Mirror_Beveled_Frame_YN =
            arg.Properties.FirstOrDefault(x =>
            x.Name.Equals(nameof(ProductFeatures.Mirror_Beveled_Frame_YN), StringComparison.OrdinalIgnoreCase))?.Value;

            component.Mirror_Frame_Finish_Family =
               arg.Properties.FirstOrDefault(x =>
                   x.Name.Equals(nameof(ProductFeatures.Mirror_Frame_Finish_Family), StringComparison.OrdinalIgnoreCase))?.Value;



            component.Mirror_Beveled_Frame_YN =
            arg.Properties.FirstOrDefault(x =>
             x.Name.Equals(nameof(ProductFeatures.Mirror_Beveled_Frame_YN), StringComparison.OrdinalIgnoreCase))?.Value;

            context.Logger.LogInformation("Current Entity Version : " + entity.Version);

            // Persist changes
            this.Commander.Pipeline<IPersistEntityPipeline>().Run(new PersistEntityArgument(entity), context);

            return Task.FromResult(arg);
        }
    }
}
