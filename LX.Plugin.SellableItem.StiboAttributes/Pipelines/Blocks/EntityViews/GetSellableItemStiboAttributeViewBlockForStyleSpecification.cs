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
    [PipelineDisplayName(SellableItemStiboAttributesConstants.Pipelines.Blocks.GetSellableItemStiboAttributeViewBlockForStyleSpecification)]
    public class GetSellableItemStiboAttributeViewBlockForStyleSpecification : PipelineBlock<EntityView, EntityView, CommercePipelineExecutionContext>
    {
        private readonly ViewCommander _viewCommander;

        public GetSellableItemStiboAttributeViewBlockForStyleSpecification(ViewCommander viewCommander)
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
                !request.ViewName.Equals(sellableitemstiboattributesviewspolicy.StyleSpecification, StringComparison.OrdinalIgnoreCase) &&
                !isVariationView &&
                !isConnectView)
            {
                return Task.FromResult(arg);
            }

            var sellableItem = (Sitecore.Commerce.Plugin.Catalog.SellableItem)request.Entity;
            var targetView = arg;
            StyleSpecification component;
            bool isEditView = false;

            // See if we are dealing with the base sellable item or one of its variations.
            var variationId = string.Empty;
            if (isVariationView && !string.IsNullOrEmpty(arg.ItemId))
            {
                variationId = arg.ItemId;
                component = sellableItem.GetVariation(variationId).GetComponent<StyleSpecification>();
            }
            else if (!string.IsNullOrEmpty(arg.ItemId) && arg.VersionedItemId != ("-1"))
            {
                variationId = arg.ItemId;
                component = sellableItem.GetVariation(variationId).GetComponent<StyleSpecification>();
            }
            else
            {
                component = sellableItem.GetComponent<StyleSpecification>(variationId);
            }

            
            #region 13. Style Specifications
            isEditView = !string.IsNullOrEmpty(arg.Action) && arg.Action.Equals(sellableitemstiboattributesviewspolicy.StyleSpecification, StringComparison.OrdinalIgnoreCase);
            if (!isEditView)
            {
                // Create a new view and add it to the current entity view.
                var view = new EntityView
                {
                    Name = context.GetPolicy<SellableItemStiboAttributesViewsPolicy>().StyleSpecification,
                    DisplayName = "Style Specifications",
                    EntityId = arg.EntityId,
                    DisplayRank = 12,
                    EntityVersion = arg.EntityVersion,
                    ItemId = variationId,
                    Icon = "piece",
                };

                arg.ChildViews.Add(view);
                targetView = view;
            }



            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Sink_Integral_Dish_Rack_YN), component.Sink_Integral_Dish_Rack_YN, component.GetDisplayName(nameof(component.Sink_Integral_Dish_Rack_YN)));


            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Drain_Included_YN), component.Drain_Included_YN, component.GetDisplayName(nameof(component.Drain_Included_YN)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Drain_Material), component.Drain_Material, component.GetDisplayName(nameof(component.Drain_Material)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Base_Threshold), component.Base_Threshold, component.GetDisplayName(nameof(component.Base_Threshold)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Door_Glass_Finish), component.Door_Glass_Finish, component.GetDisplayName(nameof(component.Door_Glass_Finish)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Bristle_Material), component.Bristle_Material, component.GetDisplayName(nameof(component.Bristle_Material)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Deck_Plate_Included_YN), component.Deck_Plate_Included_YN, component.GetDisplayName(nameof(component.Deck_Plate_Included_YN)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Shower_Trim_Type), component.Shower_Trim_Type, component.GetDisplayName(nameof(component.Shower_Trim_Type)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Shower_Valve_Type), component.Shower_Valve_Type, component.GetDisplayName(nameof(component.Shower_Valve_Type)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Shower_Wall_Installation_Type), component.Shower_Wall_Installation_Type, component.GetDisplayName(nameof(component.Shower_Wall_Installation_Type)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Showerhead_Included_YN), component.Showerhead_Included_YN, component.GetDisplayName(nameof(component.Showerhead_Included_YN)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Sink_Finish_Type), component.Sink_Finish_Type, component.GetDisplayName(nameof(component.Sink_Finish_Type)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Sink_Shape), component.Sink_Shape, component.GetDisplayName(nameof(component.Sink_Shape)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Soap_Lotion_Dispenser_YN), component.Soap_Lotion_Dispenser_YN, component.GetDisplayName(nameof(component.Soap_Lotion_Dispenser_YN)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Sink_Rack_Type), component.Sink_Rack_Type, component.GetDisplayName(nameof(component.Sink_Rack_Type)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Vanity_Hardware_Finish_Family), component.Vanity_Hardware_Finish_Family, component.GetDisplayName(nameof(component.Vanity_Hardware_Finish_Family)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Water_Filter_Included_YN), component.Water_Filter_Included_YN, component.GetDisplayName(nameof(component.Water_Filter_Included_YN)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Spray_Pattern_List), component.Spray_Pattern_List, component.GetDisplayName(nameof(component.Spray_Pattern_List)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Mirror_Frame_Material), component.Mirror_Frame_Material, component.GetDisplayName(nameof(component.Mirror_Frame_Material)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Mirror_Framed_Or_Frameless), component.Mirror_Framed_Or_Frameless, component.GetDisplayName(nameof(component.Mirror_Framed_Or_Frameless)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Mirror_Light_YN), component.Mirror_Light_YN, component.GetDisplayName(nameof(component.Mirror_Light_YN)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Mirror_Magnifying_YN), component.Mirror_Magnifying_YN, component.GetDisplayName(nameof(component.Mirror_Magnifying_YN)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Mirror_Shape), component.Mirror_Shape, component.GetDisplayName(nameof(component.Mirror_Shape)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Mirror_Telescoping_YN), component.Mirror_Telescoping_YN, component.GetDisplayName(nameof(component.Mirror_Telescoping_YN)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Mirror_Type), component.Mirror_Type, component.GetDisplayName(nameof(component.Mirror_Type)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Product_Style), component.Product_Style, component.GetDisplayName(nameof(component.Product_Style)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Shower_Arm_Style), component.Shower_Arm_Style, component.GetDisplayName(nameof(component.Shower_Arm_Style)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Shower_Diverter_Type), component.Shower_Diverter_Type, component.GetDisplayName(nameof(component.Shower_Diverter_Type)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Shower_No_Of_Spray_Settings), component.Shower_No_Of_Spray_Settings, component.GetDisplayName(nameof(component.Shower_No_Of_Spray_Settings)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Shower_No_Of_Jets), component.Shower_No_Of_Jets, component.GetDisplayName(nameof(component.Shower_No_Of_Jets)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Fitting_Material), component.Fitting_Material, component.GetDisplayName(nameof(component.Fitting_Material)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Fitting_Mount_Type), component.Fitting_Mount_Type, component.GetDisplayName(nameof(component.Fitting_Mount_Type)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Fitting_No_Of_Handles), component.Fitting_No_Of_Handles, component.GetDisplayName(nameof(component.Fitting_No_Of_Handles)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Fitting_Spout_Type), component.Fitting_Spout_Type, component.GetDisplayName(nameof(component.Fitting_Spout_Type)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Hinge_Material), component.Hinge_Material, component.GetDisplayName(nameof(component.Hinge_Material)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Fixture_Material), component.Fixture_Material, component.GetDisplayName(nameof(component.Fixture_Material)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Frame_Finish), component.Frame_Finish, component.GetDisplayName(nameof(component.Frame_Finish)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Hand_Head_Shower_Shape), component.Hand_Head_Shower_Shape, component.GetDisplayName(nameof(component.Hand_Head_Shower_Shape)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Handle_Finish), component.Handle_Finish, component.GetDisplayName(nameof(component.Handle_Finish)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Max_Occupants), component.Max_Occupants, component.GetDisplayName(nameof(component.Max_Occupants)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Flushing_Type), component.Flushing_Type, component.GetDisplayName(nameof(component.Flushing_Type)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Door_Swing), component.Door_Swing, component.GetDisplayName(nameof(component.Door_Swing)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Shower_Wall_Number_Of_Shelves), component.Shower_Wall_Number_Of_Shelves, component.GetDisplayName(nameof(component.Shower_Wall_Number_Of_Shelves)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Handle_Style), component.Handle_Style, component.GetDisplayName(nameof(component.Handle_Style)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Jet_Type), component.Jet_Type, component.GetDisplayName(nameof(component.Jet_Type)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Tub_Lighting), component.Tub_Lighting, component.GetDisplayName(nameof(component.Tub_Lighting)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Kitchen_Sink_Accessory_Grid_Features), component.Kitchen_Sink_Accessory_Grid_Features, component.GetDisplayName(nameof(component.Kitchen_Sink_Accessory_Grid_Features)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Sink_Accessory_Type), component.Sink_Accessory_Type, component.GetDisplayName(nameof(component.Sink_Accessory_Type)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Tub_Exterior_Shape), component.Tub_Exterior_Shape, component.GetDisplayName(nameof(component.Tub_Exterior_Shape)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Tub_Features), component.Tub_Features, component.GetDisplayName(nameof(component.Tub_Features)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Tub_Product_Type), component.Tub_Product_Type, component.GetDisplayName(nameof(component.Tub_Product_Type)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Bowl_Included_YN), component.Bowl_Included_YN, component.GetDisplayName(nameof(component.Bowl_Included_YN)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Item_Shape), component.Item_Shape, component.GetDisplayName(nameof(component.Item_Shape)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Kitchen_Sink_Product_Type), component.Kitchen_Sink_Product_Type, component.GetDisplayName(nameof(component.Kitchen_Sink_Product_Type)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Toilet_Activation_Lever_Location), component.Toilet_Activation_Lever_Location, component.GetDisplayName(nameof(component.Toilet_Activation_Lever_Location)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Sink_Bath_Type), component.Sink_Bath_Type, component.GetDisplayName(nameof(component.Sink_Bath_Type)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Sink_No_Of_Basins), component.Sink_No_Of_Basins, component.GetDisplayName(nameof(component.Sink_No_Of_Basins)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Trim_Kit_Included_YN), component.Trim_Kit_Included_YN, component.GetDisplayName(nameof(component.Trim_Kit_Included_YN)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Kitchen_Sink_Accessory_Rack_Type), component.Kitchen_Sink_Accessory_Rack_Type, component.GetDisplayName(nameof(component.Kitchen_Sink_Accessory_Rack_Type)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Tub_Shower_Door_Fits_Opening_Height), component.Tub_Shower_Door_Fits_Opening_Height, component.GetDisplayName(nameof(component.Tub_Shower_Door_Fits_Opening_Height)));


            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Tub_Shower_Door_Glass_Thickness), component.Tub_Shower_Door_Glass_Thickness, component.GetDisplayName(nameof(component.Tub_Shower_Door_Glass_Thickness)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Door_Glass_Style), component.Door_Glass_Style, component.GetDisplayName(nameof(component.Door_Glass_Style)));


            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Door_Type), component.Door_Type, component.GetDisplayName(nameof(component.Door_Type)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Frame_Type), component.Frame_Type, component.GetDisplayName(nameof(component.Frame_Type)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Number_Of_Flush_Valves), component.Number_Of_Flush_Valves, component.GetDisplayName(nameof(component.Number_Of_Flush_Valves)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Rough_In_Valve_Included_YN), component.Rough_In_Valve_Included_YN, component.GetDisplayName(nameof(component.Rough_In_Valve_Included_YN)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Bidet_Slow_Close_Lid_YN), component.Bidet_Slow_Close_Lid_YN, component.GetDisplayName(nameof(component.Bidet_Slow_Close_Lid_YN)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Bath_Accessory_Hanger_Type), component.Bath_Accessory_Hanger_Type, component.GetDisplayName(nameof(component.Bath_Accessory_Hanger_Type)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Bath_Accessory_Hardware_Included_YN), component.Bath_Accessory_Hardware_Included_YN, component.GetDisplayName(nameof(component.Bath_Accessory_Hardware_Included_YN)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Bath_Accessory_Holder_Type), component.Bath_Accessory_Holder_Type, component.GetDisplayName(nameof(component.Bath_Accessory_Holder_Type)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Bath_Accessory_Hook_Type), component.Bath_Accessory_Hook_Type, component.GetDisplayName(nameof(component.Bath_Accessory_Hook_Type)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Bath_Accessory_Mount_Type), component.Bath_Accessory_Mount_Type, component.GetDisplayName(nameof(component.Bath_Accessory_Mount_Type)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Bath_Accessory_Mounting_Hardware_YN), component.Bath_Accessory_Mounting_Hardware_YN, component.GetDisplayName(nameof(component.Bath_Accessory_Mounting_Hardware_YN)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Bath_Accessory_No_Of_Bars), component.Bath_Accessory_No_Of_Bars, component.GetDisplayName(nameof(component.Bath_Accessory_No_Of_Bars)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Bath_Accessory_No_Of_Chambers), component.Bath_Accessory_No_Of_Chambers, component.GetDisplayName(nameof(component.Bath_Accessory_No_Of_Chambers)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Bath_Accessory_No_Of_Hangers), component.Bath_Accessory_No_Of_Hangers, component.GetDisplayName(nameof(component.Bath_Accessory_No_Of_Hangers)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Bath_Accessory_No_Of_Pieces), component.Bath_Accessory_No_Of_Pieces, component.GetDisplayName(nameof(component.Bath_Accessory_No_Of_Pieces)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Bath_Accessory_No_Of_Hooks), component.Bath_Accessory_No_Of_Hooks, component.GetDisplayName(nameof(component.Bath_Accessory_No_Of_Hooks)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Bath_Accessory_Product_Type), component.Bath_Accessory_Product_Type, component.GetDisplayName(nameof(component.Bath_Accessory_Product_Type)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Bath_Accessory_Textured_Grip_YN), component.Bath_Accessory_Textured_Grip_YN, component.GetDisplayName(nameof(component.Bath_Accessory_Textured_Grip_YN)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Bath_Accessory_Type), component.Bath_Accessory_Type, component.GetDisplayName(nameof(component.Bath_Accessory_Type)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Shower_Corrosion_Resistant_YN), component.Shower_Corrosion_Resistant_YN, component.GetDisplayName(nameof(component.Shower_Corrosion_Resistant_YN)));


            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Temperature_Control_YN), component.Temperature_Control_YN, component.GetDisplayName(nameof(component.Temperature_Control_YN)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Side_Spray_YN), component.Side_Spray_YN, component.GetDisplayName(nameof(component.Side_Spray_YN)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Fitting_Touchless_Touch_On), component.Fitting_Touchless_Touch_On, component.GetDisplayName(nameof(component.Fitting_Touchless_Touch_On)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Foot_Operation_YN), component.Foot_Operation_YN, component.GetDisplayName(nameof(component.Foot_Operation_YN)));

            //AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Fitting_Features), component.Fitting_Features, component.GetDisplayName(nameof(component.Fitting_Features)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Pull_Down_YN), component.Pull_Down_YN, component.GetDisplayName(nameof(component.Pull_Down_YN)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Water_Efficient_YN), component.Water_Efficient_YN, component.GetDisplayName(nameof(component.Water_Efficient_YN)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Drininking_Chilled_Water_YN), component.Drininking_Chilled_Water_YN, component.GetDisplayName(nameof(component.Drininking_Chilled_Water_YN)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Drinking_Hot_Water_YN), component.Drinking_Hot_Water_YN, component.GetDisplayName(nameof(component.Drinking_Hot_Water_YN)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Drinking_Water_Dispenser_YN), component.Drinking_Water_Dispenser_YN, component.GetDisplayName(nameof(component.Drinking_Water_Dispenser_YN)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Shower_Wall_Type), component.Shower_Wall_Type, component.GetDisplayName(nameof(component.Shower_Wall_Type)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Shower_Bath_Product_Type), component.Shower_Bath_Product_Type, component.GetDisplayName(nameof(component.Shower_Bath_Product_Type)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Valve_Type), component.Valve_Type, component.GetDisplayName(nameof(component.Valve_Type)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Shower_System_Product_Type), component.Shower_System_Product_Type, component.GetDisplayName(nameof(component.Shower_System_Product_Type)));

            #endregion


            return Task.FromResult(arg);
        }
    }
}