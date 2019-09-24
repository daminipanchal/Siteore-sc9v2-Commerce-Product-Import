
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
    [PipelineDisplayName(SellableItemStiboAttributesConstants.Pipelines.Blocks.DoActionEditSellableItemContentManageableAttributeBlock)]
    public class DoActionEditSellableItemContentManageableAttributeBlock : PipelineBlock<EntityView, EntityView, CommercePipelineExecutionContext>
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
        public DoActionEditSellableItemContentManageableAttributeBlock(CommerceCommander commander)
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


            var sellableItemStiboAttributesViewsPolicy = context.GetPolicy<ContentManageableAttributesActionsPolicy>();

            // Only proceed if the right action was invoked
            if (string.IsNullOrEmpty(arg.Action) || !arg.Action.Equals(sellableItemStiboAttributesViewsPolicy.ContentManageableAttributes, StringComparison.OrdinalIgnoreCase))
            {
                return Task.FromResult(arg);
            }

            // Get the sellable item from the context
            var entity = context.CommerceContext.GetObject<Sitecore.Commerce.Plugin.Catalog.SellableItem>(x => x.Id.Equals(arg.EntityId));
            if (entity == null)
            {
                return Task.FromResult(arg);
            }
            ContentManageableAttributes component = null;
            // Get the notes component from the sellable item or its variation
            if (!string.IsNullOrWhiteSpace(arg.ItemId))
            {
                component = entity.GetVariation(arg.ItemId).GetComponent<ContentManageableAttributes>();
            }
            else
            {
                component = entity.GetComponent<ContentManageableAttributes>(arg.EntityId);
            }
            // Map entity view properties to component

            component.Material_Description_Marketing =
                arg.Properties.FirstOrDefault(x =>
                    x.Name.Equals(nameof(ContentManageableAttributes.Material_Description_Marketing), StringComparison.OrdinalIgnoreCase))?.Value;

            component.Product_Family_SAP =
                arg.Properties.FirstOrDefault(x =>
                    x.Name.Equals(nameof(ContentManageableAttributes.Product_Family_SAP), StringComparison.OrdinalIgnoreCase))?.Value;

            component.Mfg_Product_Number_SAP =
                arg.Properties.FirstOrDefault(x =>
                    x.Name.Equals(nameof(ContentManageableAttributes.Mfg_Product_Number_SAP), StringComparison.OrdinalIgnoreCase))?.Value;

            component.Marketing_Copy =
                arg.Properties.FirstOrDefault(x =>
                    x.Name.Equals(nameof(ContentManageableAttributes.Marketing_Copy), StringComparison.OrdinalIgnoreCase))?.Value;

            component.Lead_Law_Compliant_YN =
               arg.Properties.FirstOrDefault(x =>
                   x.Name.Equals(nameof(ContentManageableAttributes.Lead_Law_Compliant_YN), StringComparison.OrdinalIgnoreCase))?.Value;

            component.Products_Use_This_Part =
              arg.Properties.FirstOrDefault(x =>
                  x.Name.Equals(nameof(ContentManageableAttributes.Products_Use_This_Part), StringComparison.OrdinalIgnoreCase))?.Value;

            component.Related_Products =
               arg.Properties.FirstOrDefault(x =>
                   x.Name.Equals(nameof(ContentManageableAttributes.Related_Products), StringComparison.OrdinalIgnoreCase))?.Value;


            context.Logger.LogInformation("Current Entity Version : " + entity.Version);

            // Persist changes
            this.Commander.Pipeline<IPersistEntityPipeline>().Run(new PersistEntityArgument(entity), context);

            return Task.FromResult(arg);
        }
    }
}
