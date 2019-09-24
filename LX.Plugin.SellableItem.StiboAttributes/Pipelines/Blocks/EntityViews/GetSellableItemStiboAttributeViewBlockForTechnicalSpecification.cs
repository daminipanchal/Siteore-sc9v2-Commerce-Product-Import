
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
    [PipelineDisplayName(SellableItemStiboAttributesConstants.Pipelines.Blocks.GetSellableItemStiboAttributeViewBlockForTechnicalSpecification)]
    public class GetSellableItemStiboAttributeViewBlockForTechnicalSpecification : PipelineBlock<EntityView, EntityView, CommercePipelineExecutionContext>
    {
        private readonly ViewCommander _viewCommander;

        public GetSellableItemStiboAttributeViewBlockForTechnicalSpecification(ViewCommander viewCommander)
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
            //Make sure we target the correct View
            if (string.IsNullOrWhiteSpace(request.ViewName) ||
                     !request.ViewName.Equals(catalogViewsPolicy.Master, StringComparison.OrdinalIgnoreCase) &&
                     !request.ViewName.Equals(catalogViewsPolicy.Details, StringComparison.OrdinalIgnoreCase) &&
                     !request.ViewName.Equals(sellableitemstiboattributesviewspolicy.TechnicalSpecification, StringComparison.OrdinalIgnoreCase) && !isVariationView && !isConnectView)
            {
                return Task.FromResult(arg);
            }

            var sellableItem = (Sitecore.Commerce.Plugin.Catalog.SellableItem)request.Entity;

            // See if we are dealing with the base sellable item or one of its variations.
            var variationId = string.Empty;
            TechnicalSpecification component;
            var targetView = arg;
            bool isEditView = false;
            if (isVariationView && !string.IsNullOrEmpty(arg.ItemId))
            {
                variationId = arg.ItemId;
                component = sellableItem.GetVariation(variationId).GetComponent<TechnicalSpecification>();
            }
            else if (!string.IsNullOrEmpty(arg.ItemId) && arg.VersionedItemId != ("-1"))
            {
                variationId = arg.ItemId;
                component = sellableItem.GetVariation(variationId).GetComponent<TechnicalSpecification>();
            }
            else
            {
                component = sellableItem.GetComponent<TechnicalSpecification>(variationId);
            }

            #region 14. TechnicalSpecification
            isEditView = !string.IsNullOrEmpty(arg.Action) && arg.Action.Equals(sellableitemstiboattributesviewspolicy.TechnicalSpecification, StringComparison.OrdinalIgnoreCase);
            if (!isEditView)
            {
                // Create a new view and add it to the current entity view.
                var view = new EntityView
                {
                    Name = context.GetPolicy<SellableItemStiboAttributesViewsPolicy>().TechnicalSpecification,
                    DisplayName = "Technical Specifications",
                    EntityId = arg.EntityId,
                    DisplayRank = 13,
                    EntityVersion = arg.EntityVersion,
                    ItemId = variationId,
                    Icon = "piece",
                };

                arg.ChildViews.Add(view);
                targetView = view;
            }

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Mirror_Hardware_Included_YN), component.Mirror_Hardware_Included_YN, component.GetDisplayName(nameof(component.Mirror_Hardware_Included_YN)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Mirror_Mount_Location), component.Mirror_Mount_Location, component.GetDisplayName(nameof(component.Mirror_Mount_Location)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Number_Output_Valves_Connection), component.Number_Output_Valves_Connection, component.GetDisplayName(nameof(component.Number_Output_Valves_Connection)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Output_Connection_Type), component.Output_Connection_Type, component.GetDisplayName(nameof(component.Output_Connection_Type)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Shower_Rough_In_Valve), component.Shower_Rough_In_Valve, component.GetDisplayName(nameof(component.Shower_Rough_In_Valve)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Fitting_Connection_Size_Input), component.Fitting_Connection_Size_Input, component.GetDisplayName(nameof(component.Fitting_Connection_Size_Input)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Fitting_Handle_Height), component.Fitting_Handle_Height, component.GetDisplayName(nameof(component.Fitting_Handle_Height)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Fitting_Handle_Type), component.Fitting_Handle_Type, component.GetDisplayName(nameof(component.Fitting_Handle_Type)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Fitting_Hose_Length), component.Fitting_Hose_Length, component.GetDisplayName(nameof(component.Fitting_Hose_Length)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Fitting_Max_Deck_Thickness), component.Fitting_Max_Deck_Thickness, component.GetDisplayName(nameof(component.Fitting_Max_Deck_Thickness)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Fitting_Max_Deck_Thickness_w_Escutcheon), component.Fitting_Max_Deck_Thickness_w_Escutcheon, component.GetDisplayName(nameof(component.Fitting_Max_Deck_Thickness_w_Escutcheon)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Fitting_No_Of_Holes_Required), component.Fitting_No_Of_Holes_Required, component.GetDisplayName(nameof(component.Fitting_No_Of_Holes_Required)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Fitting_Speed_Connect_YN), component.Fitting_Speed_Connect_YN, component.GetDisplayName(nameof(component.Fitting_Speed_Connect_YN)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Fitting_Spout_Height), component.Fitting_Spout_Height, component.GetDisplayName(nameof(component.Fitting_Spout_Height)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Tub_Depth), component.Tub_Depth, component.GetDisplayName(nameof(component.Tub_Depth)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Tub_Overflow_Height), component.Tub_Overflow_Height, component.GetDisplayName(nameof(component.Tub_Overflow_Height)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Tub_Water_Depth), component.Tub_Water_Depth, component.GetDisplayName(nameof(component.Tub_Water_Depth)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Fitting_Spout_Reach), component.Fitting_Spout_Reach, component.GetDisplayName(nameof(component.Fitting_Spout_Reach)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Fitting_Swivel_Degrees), component.Fitting_Swivel_Degrees, component.GetDisplayName(nameof(component.Fitting_Swivel_Degrees)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Fitting_Valve_Needed_For_Install_YN), component.Fitting_Valve_Needed_For_Install_YN, component.GetDisplayName(nameof(component.Fitting_Valve_Needed_For_Install_YN)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.FlowRateGPC), component.FlowRateGPC, component.GetDisplayName(nameof(component.FlowRateGPC)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Handle_Length), component.Handle_Length, component.GetDisplayName(nameof(component.Handle_Length)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Operating_Pressure_Static), component.Operating_Pressure_Static, component.GetDisplayName(nameof(component.Operating_Pressure_Static)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Handle_To_Handle_Measurement), component.Handle_To_Handle_Measurement, component.GetDisplayName(nameof(component.Handle_To_Handle_Measurement)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Fitting_Valve_Material), component.Fitting_Valve_Material, component.GetDisplayName(nameof(component.Fitting_Valve_Material)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Hose_Included_YN), component.Hose_Included_YN, component.GetDisplayName(nameof(component.Hose_Included_YN)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Input_Connection_Type), component.Input_Connection_Type, component.GetDisplayName(nameof(component.Input_Connection_Type)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Installation_Hardware_Included), component.Installation_Hardware_Included, component.GetDisplayName(nameof(component.Installation_Hardware_Included)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Tub_Wattage), component.Tub_Wattage, component.GetDisplayName(nameof(component.Tub_Wattage)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Tub_Blower_Placement), component.Tub_Blower_Placement, component.GetDisplayName(nameof(component.Tub_Blower_Placement)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Tub_Pump_Placement), component.Tub_Pump_Placement, component.GetDisplayName(nameof(component.Tub_Pump_Placement)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Tub_Pump_Speeds), component.Tub_Pump_Speeds, component.GetDisplayName(nameof(component.Tub_Pump_Speeds)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Tub_Sound_Dampening), component.Tub_Sound_Dampening, component.GetDisplayName(nameof(component.Tub_Sound_Dampening)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Sink_Predrilled_Holes_YN), component.Sink_Predrilled_Holes_YN, component.GetDisplayName(nameof(component.Sink_Predrilled_Holes_YN)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Number_Of_Toilet_Trapways), component.Number_Of_Toilet_Trapways, component.GetDisplayName(nameof(component.Number_Of_Toilet_Trapways)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Number_Of_Toilet_Flush_Valves), component.Number_Of_Toilet_Flush_Valves, component.GetDisplayName(nameof(component.Number_Of_Toilet_Flush_Valves)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Mold), component.Mold, component.GetDisplayName(nameof(component.Mold)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Bidet_No_Of_Dryer_Settings), component.Bidet_No_Of_Dryer_Settings, component.GetDisplayName(nameof(component.Bidet_No_Of_Dryer_Settings)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Bidet_No_Of_Spray_Nozzles), component.Bidet_No_Of_Spray_Nozzles, component.GetDisplayName(nameof(component.Bidet_No_Of_Spray_Nozzles)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Bidet_No_Of_Water_Control_Settings), component.Bidet_No_Of_Water_Control_Settings, component.GetDisplayName(nameof(component.Bidet_No_Of_Water_Control_Settings)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Bidet_Remote_Included_YN), component.Bidet_Remote_Included_YN, component.GetDisplayName(nameof(component.Bidet_Remote_Included_YN)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Bath_Fitting_Bidet_Spray_Type), component.Bath_Fitting_Bidet_Spray_Type, component.GetDisplayName(nameof(component.Bath_Fitting_Bidet_Spray_Type)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Power_Cord_Length), component.Power_Cord_Length, component.GetDisplayName(nameof(component.Power_Cord_Length)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Toilet_Seat_Front_Type), component.Toilet_Seat_Front_Type, component.GetDisplayName(nameof(component.Toilet_Seat_Front_Type)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Flushing_Mechanism), component.Flushing_Mechanism, component.GetDisplayName(nameof(component.Flushing_Mechanism)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.No_Of_Batteries_Included), component.No_Of_Batteries_Included, component.GetDisplayName(nameof(component.No_Of_Batteries_Included)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Shower_Base_No_Finished_Sides), component.Shower_Base_No_Finished_Sides, component.GetDisplayName(nameof(component.Shower_Base_No_Finished_Sides)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Shower_Base_Number_Of_Curbs), component.Shower_Base_Number_Of_Curbs, component.GetDisplayName(nameof(component.Shower_Base_Number_Of_Curbs)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Door_Glass_Thickness), component.Door_Glass_Thickness, component.GetDisplayName(nameof(component.Door_Glass_Thickness)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Door_Hinged_YN), component.Door_Hinged_YN, component.GetDisplayName(nameof(component.Door_Hinged_YN)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Door_Usage), component.Door_Usage, component.GetDisplayName(nameof(component.Door_Usage)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Drain_Location), component.Drain_Location, component.GetDisplayName(nameof(component.Drain_Location)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Towel_Bar_Included_YN), component.Towel_Bar_Included_YN, component.GetDisplayName(nameof(component.Towel_Bar_Included_YN)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Tub_Shower_Door_Width), component.Tub_Shower_Door_Width, component.GetDisplayName(nameof(component.Tub_Shower_Door_Width)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Shower_Wall_Caulkless_YN), component.Shower_Wall_Caulkless_YN, component.GetDisplayName(nameof(component.Shower_Wall_Caulkless_YN)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Shower_Wall_Construction), component.Shower_Wall_Construction, component.GetDisplayName(nameof(component.Shower_Wall_Construction)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Shower_Wall_Surface), component.Shower_Wall_Surface, component.GetDisplayName(nameof(component.Shower_Wall_Surface)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Bowl_Right_Side_Below_Counter_Depth), component.Bowl_Right_Side_Below_Counter_Depth, component.GetDisplayName(nameof(component.Bowl_Right_Side_Below_Counter_Depth)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Sink_Cut_Out_Below_Counter_Depth), component.Sink_Cut_Out_Below_Counter_Depth, component.GetDisplayName(nameof(component.Sink_Cut_Out_Below_Counter_Depth)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Sink_Cut_Out_Front_To_Back_Width), component.Sink_Cut_Out_Front_To_Back_Width, component.GetDisplayName(nameof(component.Sink_Cut_Out_Front_To_Back_Width)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Sink_Cut_Out_Left_To_Right_Length), component.Sink_Cut_Out_Left_To_Right_Length, component.GetDisplayName(nameof(component.Sink_Cut_Out_Left_To_Right_Length)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Sink_Depth_To_Overflow), component.Sink_Depth_To_Overflow, component.GetDisplayName(nameof(component.Sink_Depth_To_Overflow)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Sink_Minimum_Cabinet_Size), component.Sink_Minimum_Cabinet_Size, component.GetDisplayName(nameof(component.Sink_Minimum_Cabinet_Size)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Sink_Overflow_Location), component.Sink_Overflow_Location, component.GetDisplayName(nameof(component.Sink_Overflow_Location)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Tub_Blower), component.Tub_Blower, component.GetDisplayName(nameof(component.Tub_Blower)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Tub_Pump_Amperage), component.Tub_Pump_Amperage, component.GetDisplayName(nameof(component.Tub_Pump_Amperage)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Tub_Water_Capacity), component.Tub_Water_Capacity, component.GetDisplayName(nameof(component.Tub_Water_Capacity)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Sink_Bowl_Below_Counter_Depth), component.Sink_Bowl_Below_Counter_Depth, component.GetDisplayName(nameof(component.Sink_Bowl_Below_Counter_Depth)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Sink_Front_To_Back_Width), component.Sink_Front_To_Back_Width, component.GetDisplayName(nameof(component.Sink_Front_To_Back_Width)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Sink_Left_To_Right_Length), component.Sink_Left_To_Right_Length, component.GetDisplayName(nameof(component.Sink_Left_To_Right_Length)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Sink_Top_To_Bottom_Depth), component.Sink_Top_To_Bottom_Depth, component.GetDisplayName(nameof(component.Sink_Top_To_Bottom_Depth)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Sump_Length), component.Sump_Length, component.GetDisplayName(nameof(component.Sump_Length)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Sump_Width), component.Sump_Width, component.GetDisplayName(nameof(component.Sump_Width)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Soaking_Depth), component.Soaking_Depth, component.GetDisplayName(nameof(component.Soaking_Depth)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Flow_Rate), component.Flow_Rate, component.GetDisplayName(nameof(component.Flow_Rate)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Flush_Technology), component.Flush_Technology, component.GetDisplayName(nameof(component.Flush_Technology)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Sink_Bowl_Split), component.Sink_Bowl_Split, component.GetDisplayName(nameof(component.Sink_Bowl_Split)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.MAP_Performance_Rating), component.MAP_Performance_Rating, component.GetDisplayName(nameof(component.MAP_Performance_Rating)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Bowl_Height_Without_Seat), component.Bowl_Height_Without_Seat, component.GetDisplayName(nameof(component.Bowl_Height_Without_Seat)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Tub_Shower_Door_Height), component.Tub_Shower_Door_Height, component.GetDisplayName(nameof(component.Tub_Shower_Door_Height)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Tub_Shower_Door_Opening_Width_Max), component.Tub_Shower_Door_Opening_Width_Max, component.GetDisplayName(nameof(component.Tub_Shower_Door_Opening_Width_Max)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Tub_Shower_Door_Opening_Width_Min), component.Tub_Shower_Door_Opening_Width_Min, component.GetDisplayName(nameof(component.Tub_Shower_Door_Opening_Width_Min)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Flush_Valve_Size), component.Flush_Valve_Size, component.GetDisplayName(nameof(component.Flush_Valve_Size)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Operating_Pressure_Flushing), component.Operating_Pressure_Flushing, component.GetDisplayName(nameof(component.Operating_Pressure_Flushing)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Rough_In_Size), component.Rough_In_Size, component.GetDisplayName(nameof(component.Rough_In_Size)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Toilet_Bowl_Height_With_Seat), component.Toilet_Bowl_Height_With_Seat, component.GetDisplayName(nameof(component.Toilet_Bowl_Height_With_Seat)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Toilet_Trapway_Diameter), component.Toilet_Trapway_Diameter, component.GetDisplayName(nameof(component.Toilet_Trapway_Diameter)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Battery_Weight), component.Battery_Weight, component.GetDisplayName(nameof(component.Battery_Weight)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Bidet_No_Of_Heat_Settings), component.Bidet_No_Of_Heat_Settings, component.GetDisplayName(nameof(component.Bidet_No_Of_Heat_Settings)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Chemical_Composition_Of_Battery), component.Chemical_Composition_Of_Battery, component.GetDisplayName(nameof(component.Chemical_Composition_Of_Battery)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Toilet_Seat_Bolt_Spread), component.Toilet_Seat_Bolt_Spread, component.GetDisplayName(nameof(component.Toilet_Seat_Bolt_Spread)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Escutcheon_Internal_Diameter), component.Escutcheon_Internal_Diameter, component.GetDisplayName(nameof(component.Escutcheon_Internal_Diameter)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Number_Input_Valves_Connection), component.Number_Input_Valves_Connection, component.GetDisplayName(nameof(component.Number_Input_Valves_Connection)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Bath_Accessory_Weight_Capacity), component.Bath_Accessory_Weight_Capacity, component.GetDisplayName(nameof(component.Bath_Accessory_Weight_Capacity)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Bristle_Stiffness), component.Bristle_Stiffness, component.GetDisplayName(nameof(component.Bristle_Stiffness)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Bristle_Length), component.Bristle_Length, component.GetDisplayName(nameof(component.Bristle_Length)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Shower_Arm_Reach), component.Shower_Arm_Reach, component.GetDisplayName(nameof(component.Shower_Arm_Reach)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Shower_Wall_Bar_Length), component.Shower_Wall_Bar_Length, component.GetDisplayName(nameof(component.Shower_Wall_Bar_Length)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Concealed_Or_Exposed_Flush_Valve), component.Concealed_Or_Exposed_Flush_Valve, component.GetDisplayName(nameof(component.Concealed_Or_Exposed_Flush_Valve)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Assembly_Required_YN), component.Assembly_Required_YN, component.GetDisplayName(nameof(component.Assembly_Required_YN)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Bath_Accessory_Installation_Type), component.Bath_Accessory_Installation_Type, component.GetDisplayName(nameof(component.Bath_Accessory_Installation_Type)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Bath_Shower_Diameter), component.Bath_Shower_Diameter, component.GetDisplayName(nameof(component.Bath_Shower_Diameter)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Bath_Shower_Maximum_Deck_Thickness), component.Bath_Shower_Maximum_Deck_Thickness, component.GetDisplayName(nameof(component.Bath_Shower_Maximum_Deck_Thickness)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Bath_Shower_Showerhead_Face_Diameter), component.Bath_Shower_Showerhead_Face_Diameter, component.GetDisplayName(nameof(component.Bath_Shower_Showerhead_Face_Diameter)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Bath_Shower_Sprayer_Face_Diameter), component.Bath_Shower_Sprayer_Face_Diameter, component.GetDisplayName(nameof(component.Bath_Shower_Sprayer_Face_Diameter)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Batteries_Included_YN), component.Batteries_Included_YN, component.GetDisplayName(nameof(component.Batteries_Included_YN)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Compliant_Product_Substitute), component.Compliant_Product_Substitute, component.GetDisplayName(nameof(component.Compliant_Product_Substitute)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.EA_Each_Weight), component.EA_Each_Weight, component.GetDisplayName(nameof(component.EA_Each_Weight)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Faucet_Hole_Spacing), component.Faucet_Hole_Spacing, component.GetDisplayName(nameof(component.Faucet_Hole_Spacing)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Shower_Wall_Bar_Installation), component.Shower_Wall_Bar_Installation, component.GetDisplayName(nameof(component.Shower_Wall_Bar_Installation)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Tub_Interior_Shape), component.Tub_Interior_Shape, component.GetDisplayName(nameof(component.Tub_Interior_Shape)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Tub_Voltage), component.Tub_Voltage, component.GetDisplayName(nameof(component.Tub_Voltage)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Valve_Application_YN), component.Valve_Application_YN, component.GetDisplayName(nameof(component.Valve_Application_YN)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Valve_Indoor_Outdoor), component.Valve_Indoor_Outdoor, component.GetDisplayName(nameof(component.Valve_Indoor_Outdoor)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Valve_Input_Diameter), component.Valve_Input_Diameter, component.GetDisplayName(nameof(component.Valve_Input_Diameter)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Valve_Output_Diameter), component.Valve_Output_Diameter, component.GetDisplayName(nameof(component.Valve_Output_Diameter)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Valves_Maximum_Pressure), component.Valves_Maximum_Pressure, component.GetDisplayName(nameof(component.Valves_Maximum_Pressure)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Valves_Maximum_Working_Temperature), component.Valves_Maximum_Working_Temperature, component.GetDisplayName(nameof(component.Valves_Maximum_Working_Temperature)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Volume_Control_YN), component.Volume_Control_YN, component.GetDisplayName(nameof(component.Volume_Control_YN)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Volume_SAP), component.Volume_SAP, component.GetDisplayName(nameof(component.Volume_SAP)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Weight_Capacity), component.Weight_Capacity, component.GetDisplayName(nameof(component.Weight_Capacity)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Tank_Included_YN), component.Tank_Included_YN, component.GetDisplayName(nameof(component.Tank_Included_YN)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Rough_In_Valve_Features), component.Rough_In_Valve_Features, component.GetDisplayName(nameof(component.Rough_In_Valve_Features)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.ADA_YN), component.ADA_YN, component.GetDisplayName(nameof(component.ADA_YN)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Barrier_Free), component.Barrier_Free, component.GetDisplayName(nameof(component.Barrier_Free)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Cal_Green_YN), component.Cal_Green_YN, component.GetDisplayName(nameof(component.Cal_Green_YN)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.CEC_Certified_YN), component.CEC_Certified_YN, component.GetDisplayName(nameof(component.CEC_Certified_YN)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Certification_UL), component.Certification_UL, component.GetDisplayName(nameof(component.Certification_UL)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Certification_NSF), component.Certification_NSF, component.GetDisplayName(nameof(component.Certification_NSF)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.EPA_Watersense_YN), component.EPA_Watersense_YN, component.GetDisplayName(nameof(component.EPA_Watersense_YN)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Proposition_65_Disclosure_YN), component.Proposition_65_Disclosure_YN, component.GetDisplayName(nameof(component.Proposition_65_Disclosure_YN)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Smoke_And_Flame_Compliant), component.Smoke_And_Flame_Compliant, component.GetDisplayName(nameof(component.Smoke_And_Flame_Compliant)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Certification_CAN_UL), component.Certification_CAN_UL, component.GetDisplayName(nameof(component.Certification_CAN_UL)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Massachusetts_Plumbing_Board_YN), component.Massachusetts_Plumbing_Board_YN, component.GetDisplayName(nameof(component.Massachusetts_Plumbing_Board_YN)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Toilet_Seat_Length_Inside), component.Toilet_Seat_Length_Inside, component.GetDisplayName(nameof(component.Toilet_Seat_Length_Inside)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Toilet_Seat_Width_Inside), component.Toilet_Seat_Width_Inside, component.GetDisplayName(nameof(component.Toilet_Seat_Width_Inside)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Tub_Drain_Opening_Size), component.Tub_Drain_Opening_Size, component.GetDisplayName(nameof(component.Tub_Drain_Opening_Size)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Tub_Shower_Base_Threshold_Height), component.Tub_Shower_Base_Threshold_Height, component.GetDisplayName(nameof(component.Tub_Shower_Base_Threshold_Height)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Vanity_Cabinet_Depth), component.Vanity_Cabinet_Depth, component.GetDisplayName(nameof(component.Vanity_Cabinet_Depth)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Vanity_Cabinet_Height), component.Vanity_Cabinet_Height, component.GetDisplayName(nameof(component.Vanity_Cabinet_Height)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Vanity_Cabinet_Width), component.Vanity_Cabinet_Width, component.GetDisplayName(nameof(component.Vanity_Cabinet_Width)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Vanity_Assembled_Depth), component.Vanity_Assembled_Depth, component.GetDisplayName(nameof(component.Vanity_Assembled_Depth)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Vanity_Assembled_Height), component.Vanity_Assembled_Height, component.GetDisplayName(nameof(component.Vanity_Assembled_Height)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Vanity_Assembled_Weight), component.Vanity_Assembled_Weight, component.GetDisplayName(nameof(component.Vanity_Assembled_Weight)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Vanity_Assembled_Width), component.Vanity_Assembled_Width, component.GetDisplayName(nameof(component.Vanity_Assembled_Width)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Product_Height), component.Product_Height, component.GetDisplayName(nameof(component.Product_Height)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Product_Weight), component.Product_Weight, component.GetDisplayName(nameof(component.Product_Weight)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Product_Length), component.Product_Length, component.GetDisplayName(nameof(component.Product_Length)));

            AddSellableItemProperties.AddPropertiesToViewWithSection(targetView, component, Models.ProductContsants.NonEditableField, nameof(component.Product_Width), component.Product_Width, component.GetDisplayName(nameof(component.Product_Width)));
            #endregion


            return Task.FromResult(arg);
        }
    }
}