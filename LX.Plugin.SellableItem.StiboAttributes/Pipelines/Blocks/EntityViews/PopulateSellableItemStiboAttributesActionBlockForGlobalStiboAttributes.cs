using LX.Plugin.SellableItem.StiboAttributes.Policies;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.EntityViews;
using Sitecore.Framework.Conditions;
using Sitecore.Framework.Pipelines;
using System;
using System.Threading.Tasks;

namespace LX.Plugin.SellableItem.StiboAttributes.Pipelines.Blocks.EntityViews
{
    [PipelineDisplayName(SellableItemStiboAttributesConstants.Pipelines.Blocks.PopulateSellableItemStiboAttributesActionBlockForGlobalStiboAttributes)]
    public class PopulateSellableItemStiboAttributesActionBlockForGlobalStiboAttributes :
   PipelineBlock<EntityView, EntityView, CommercePipelineExecutionContext>
    {

        public override Task<EntityView> Run(EntityView arg, CommercePipelineExecutionContext context)
        {
            Condition.Requires(arg).IsNotNull($"{Name}: The argument cannot be null.");

            var viewsPolicy = context.GetPolicy<SellableItemStiboAttributesViewsPolicy>();

            if (string.IsNullOrEmpty(arg?.Name) || !arg.Name.Equals(viewsPolicy.GlobalStiboAttributes, StringComparison.OrdinalIgnoreCase))
            {
                return Task.FromResult(arg);
            }

            var actionPolicy = arg.GetPolicy<ActionsPolicy>();

            actionPolicy.Actions.Add(
                new EntityActionView
                {
                    Name = context.GetPolicy<SellableItemStiboAttributesActionsPolicy>().GlobalStiboAttributes,
                    DisplayName = "Edit Global STIBO Attributes",
                    Description = "Edit Global STIBO Attributes",
                    IsEnabled = true,
                    EntityView = arg.Name,
                    Icon = "edit"
                });

            return Task.FromResult(arg);

        }
    }
}
