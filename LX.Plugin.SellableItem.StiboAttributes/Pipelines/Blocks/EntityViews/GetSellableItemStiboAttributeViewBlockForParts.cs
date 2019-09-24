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
    [PipelineDisplayName(SellableItemStiboAttributesConstants.Pipelines.Blocks.GetSellableItemStiboAttributeViewBlockForParts)]
    public class GetSellableItemStiboAttributeViewBlockForParts : PipelineBlock<EntityView, EntityView, CommercePipelineExecutionContext>
    {
        private readonly ViewCommander _viewCommander;

        public GetSellableItemStiboAttributeViewBlockForParts(ViewCommander viewCommander)
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

            var sellableItem = (Sitecore.Commerce.Plugin.Catalog.SellableItem)request.Entity;

            // See if we are dealing with the base sellable item or one of its variations.
            var variationId = string.Empty;
            if (isVariationView && !string.IsNullOrEmpty(arg.ItemId))
            {
                variationId = arg.ItemId;
            }
            //Make sure we target the correct View
            if (string.IsNullOrWhiteSpace(request.ViewName) ||
                     !request.ViewName.Equals(catalogViewsPolicy.Master, StringComparison.OrdinalIgnoreCase) &&
                     !request.ViewName.Equals(catalogViewsPolicy.Details, StringComparison.OrdinalIgnoreCase) &&
                     !request.ViewName.Equals(sellableitemstiboattributesviewspolicy.Parts, StringComparison.OrdinalIgnoreCase) && !isVariationView && !isConnectView)
            {
                return Task.FromResult(arg);
            }
            var targetView = arg;
            bool isEditView = false;
            Parts component;
            if (isVariationView && !string.IsNullOrEmpty(arg.ItemId))
            {
                variationId = arg.ItemId;
                component = sellableItem.GetVariation(variationId).GetComponent<Parts>();
            }
            else if (!string.IsNullOrEmpty(arg.ItemId) && arg.VersionedItemId != ("-1"))
            {
                variationId = arg.ItemId;
                component = sellableItem.GetVariation(variationId).GetComponent<Parts>();
            }
            else
            {
                component = sellableItem.GetComponent<Parts>(variationId);
            }

            #region 18. Parts
            isEditView = !string.IsNullOrEmpty(arg.Action) && arg.Action.Equals(sellableitemstiboattributesviewspolicy.Parts, StringComparison.OrdinalIgnoreCase);
            if (!isEditView)
            {
                // Create a new view and add it to the current entity view.
                var view = new EntityView
                {
                    Name = context.GetPolicy<SellableItemStiboAttributesViewsPolicy>().Parts,
                    DisplayName = "Parts",
                    EntityId = arg.EntityId,
                    DisplayRank = 9,
                    EntityVersion = arg.EntityVersion,
                    ItemId = variationId,
                    Icon = "piece",
                };

                arg.ChildViews.Add(view);
                targetView = view;
            }

            AddSellableItemProperties.AddLinkPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Replacement_Material), component.Replacement_Material_URL, component.Replacement_Material_URL, component.GetDisplayName(nameof(component.Replacement_Material)), Models.CommerceUiType.Html);

            AddSellableItemProperties.AddLinkPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Replacement_Parts), component.Replacement_Parts_URL, component.Replacement_Parts_URL, component.GetDisplayName(nameof(component.Replacement_Parts)), Models.CommerceUiType.Html);

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Included_Components), component.Included_Components, component.GetDisplayName(nameof(component.Included_Components)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Replacement_Parts_Doc_URL), component.Replacement_Parts_Doc_URL, component.GetDisplayName(nameof(component.Replacement_Parts_Doc_URL)));
            #endregion


            return Task.FromResult(arg);
        }
    }
}