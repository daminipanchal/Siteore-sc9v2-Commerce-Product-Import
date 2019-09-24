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
    [PipelineDisplayName(SellableItemStiboAttributesConstants.Pipelines.Blocks.GetSellableItemStiboAttributeViewBlockForSellableItemFacets)]
    public class GetSellableItemStiboAttributeViewBlockForSellableItemFacets : PipelineBlock<EntityView, EntityView, CommercePipelineExecutionContext>
    {
        private readonly ViewCommander _viewCommander;

        public GetSellableItemStiboAttributeViewBlockForSellableItemFacets(ViewCommander viewCommander)
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

            // Make sure that we target the correct views
            if (string.IsNullOrEmpty(request.ViewName) ||
                !request.ViewName.Equals(catalogViewsPolicy.Master, StringComparison.OrdinalIgnoreCase) &&
                !request.ViewName.Equals(catalogViewsPolicy.Details, StringComparison.OrdinalIgnoreCase) &&
                !request.ViewName.Equals(sellableitemstiboattributesviewspolicy.SellableItemFacets, StringComparison.OrdinalIgnoreCase) &&
                !isVariationView &&
                !isConnectView)
            {
                return Task.FromResult(arg);
            }

            var sellableItem = (Sitecore.Commerce.Plugin.Catalog.SellableItem)request.Entity;
            var targetView = arg;
            SellableItemFacets component;
            bool isEditView = false;

            // See if we are dealing with the base sellable item or one of its variations.
            var variationId = string.Empty;
            if (isVariationView && !string.IsNullOrEmpty(arg.ItemId))
            {
                variationId = arg.ItemId;
                component = sellableItem.GetVariation(variationId).GetComponent<SellableItemFacets>();
            }
            else if (!string.IsNullOrEmpty(arg.ItemId) && arg.VersionedItemId != ("-1"))
            {
                variationId = arg.ItemId;
                component = sellableItem.GetVariation(variationId).GetComponent<SellableItemFacets>();
            }
            else
            {
                component = sellableItem.GetComponent<SellableItemFacets>(variationId);
            }

            
            #region 15. Sellable Item Facets
            isEditView = !string.IsNullOrEmpty(arg.Action) && arg.Action.Equals(sellableitemstiboattributesviewspolicy.SellableItemFacets, StringComparison.OrdinalIgnoreCase);
            if (!isEditView)
            {
                // Create a new view and add it to the current entity view.
                var view = new EntityView
                {
                    Name = context.GetPolicy<SellableItemStiboAttributesViewsPolicy>().SellableItemFacets,
                    DisplayName = "Product Facets",
                    EntityId = arg.EntityId,
                    DisplayRank = 10,
                    EntityVersion = arg.EntityVersion,
                    ItemId = variationId,
                    Icon = "piece",
                };

                arg.ChildViews.Add(view);
                targetView = view;
            }
            

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Item_Shape), component.Item_Shape, component.GetDisplayName(nameof(component.Item_Shape)));
            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Features), component.Features, component.GetDisplayName(nameof(component.Features)));
            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Certifications), component.Certifications, component.GetDisplayName(nameof(component.Certifications)));
            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Water_Type), component.Water_Type, component.GetDisplayName(nameof(component.Water_Type)));
            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Shower_Type), component.Shower_Type, component.GetDisplayName(nameof(component.Shower_Type)));
            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Dimensions), component.Dimensions, component.GetDisplayName(nameof(component.Dimensions)));
            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Flow_Rate), component.Flow_Rate, component.GetDisplayName(nameof(component.Flow_Rate)));
            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Tub_Dimensions), component.Tub_Dimensions, component.GetDisplayName(nameof(component.Tub_Dimensions)));
            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Vanity_Assembled_Dimensions), component.Vanity_Assembled_Dimensions, component.GetDisplayName(nameof(component.Vanity_Assembled_Dimensions)));
            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Vanity_Cabinet_Dimensions), component.Vanity_Cabinet_Dimensions, component.GetDisplayName(nameof(component.Vanity_Cabinet_Dimensions)));
            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Drain_Opening_Size), component.Drain_Opening_Size, component.GetDisplayName(nameof(component.Drain_Opening_Size)));
            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Toilet_Seat_Inside_Length), component.Toilet_Seat_Inside_Length, component.GetDisplayName(nameof(component.Toilet_Seat_Inside_Length)));

            #endregion


            return Task.FromResult(arg);
        }
    }
}