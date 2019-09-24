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
    [PipelineDisplayName(SellableItemStiboAttributesConstants.Pipelines.Blocks.DoActionEditSellableItemTechnicalSpecificationBlock)]
    public class DoActionEditSellableItemTechnicalSpecificationBlock : PipelineBlock<EntityView, EntityView, CommercePipelineExecutionContext>
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
        public DoActionEditSellableItemTechnicalSpecificationBlock(CommerceCommander commander)
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


            var sellableItemStiboAttributesViewsPolicy = context.GetPolicy<TechnicalSpecificationActionsPolicy>();

            // Only proceed if the right action was invoked
            if (string.IsNullOrEmpty(arg.Action) || !arg.Action.Equals(sellableItemStiboAttributesViewsPolicy.TechnicalSpecification, StringComparison.OrdinalIgnoreCase))
            {
                return Task.FromResult(arg);
            }

            // Get the sellable item from the context
            var entity = context.CommerceContext.GetObject<Sitecore.Commerce.Plugin.Catalog.SellableItem>(x => x.Id.Equals(arg.EntityId));
            if (entity == null)
            {
                return Task.FromResult(arg);
            }

            TechnicalSpecification component;
            // Get the notes component from the sellable item or its variation
            if (!string.IsNullOrWhiteSpace(arg.ItemId))
            {
                component = entity.GetVariation(arg.ItemId).GetComponent<TechnicalSpecification>();
            }
            else
            {
                component = entity.GetComponent<TechnicalSpecification>();
            }

            int tempIntValue = 0;
            decimal tempDoubleValue = 0;

            component.Mirror_Hardware_Included_YN =
              arg.Properties.FirstOrDefault(x =>
                  x.Name.Equals(nameof(TechnicalSpecification.Mirror_Hardware_Included_YN), StringComparison.OrdinalIgnoreCase))?.Value;

            component.Mirror_Mount_Location =
                arg.Properties.FirstOrDefault(x =>
                    x.Name.Equals(nameof(TechnicalSpecification.Mirror_Mount_Location), StringComparison.OrdinalIgnoreCase))?.Value;

            ViewProperty viewProperty = arg.Properties.FirstOrDefault(x => x.Name.Equals(nameof(TechnicalSpecification.Number_Output_Valves_Connection), StringComparison.OrdinalIgnoreCase));
            if (!string.IsNullOrEmpty(viewProperty?.Value) && int.TryParse(viewProperty.Value, out tempIntValue))
                component.Number_Output_Valves_Connection = tempIntValue;

            component.Output_Connection_Type =
                arg.Properties.FirstOrDefault(x =>
                    x.Name.Equals(nameof(TechnicalSpecification.Output_Connection_Type), StringComparison.OrdinalIgnoreCase))?.Value;


            viewProperty = arg.Properties.FirstOrDefault(x => x.Name.Equals(nameof(TechnicalSpecification.Shower_Rough_In_Valve), StringComparison.OrdinalIgnoreCase));
            if (!string.IsNullOrEmpty(viewProperty?.Value) && int.TryParse(viewProperty.Value, out tempIntValue))
                component.Shower_Rough_In_Valve = tempIntValue;

            component.Fitting_Connection_Size_Input =
                arg.Properties.FirstOrDefault(x =>
                    x.Name.Equals(nameof(TechnicalSpecification.Fitting_Connection_Size_Input), StringComparison.OrdinalIgnoreCase))?.Value;

            component.Fitting_Handle_Height =
                arg.Properties.FirstOrDefault(x =>
                    x.Name.Equals(nameof(TechnicalSpecification.Fitting_Handle_Height), StringComparison.OrdinalIgnoreCase))?.Value;


            component.Fitting_Handle_Type =
                arg.Properties.FirstOrDefault(x =>
                    x.Name.Equals(nameof(TechnicalSpecification.Fitting_Handle_Type), StringComparison.OrdinalIgnoreCase))?.Value;

            viewProperty = arg.Properties.FirstOrDefault(x => x.Name.Equals(nameof(TechnicalSpecification.Fitting_Hose_Length), StringComparison.OrdinalIgnoreCase));
            if (!string.IsNullOrEmpty(viewProperty?.Value) && decimal.TryParse(viewProperty.Value, out tempDoubleValue))
                component.Fitting_Hose_Length = tempDoubleValue;


            component.Fitting_Max_Deck_Thickness =
               arg.Properties.FirstOrDefault(x =>
                   x.Name.Equals(nameof(TechnicalSpecification.Fitting_Max_Deck_Thickness), StringComparison.OrdinalIgnoreCase))?.Value;

            viewProperty = arg.Properties.FirstOrDefault(x => x.Name.Equals(nameof(TechnicalSpecification.Fitting_Max_Deck_Thickness_w_Escutcheon), StringComparison.OrdinalIgnoreCase));
            if (!string.IsNullOrEmpty(viewProperty?.Value) && int.TryParse(viewProperty.Value, out tempIntValue))
                component.Fitting_Max_Deck_Thickness_w_Escutcheon = tempIntValue;

            viewProperty = arg.Properties.FirstOrDefault(x => x.Name.Equals(nameof(TechnicalSpecification.Fitting_No_Of_Holes_Required), StringComparison.OrdinalIgnoreCase));
            if (!string.IsNullOrEmpty(viewProperty?.Value) && int.TryParse(viewProperty.Value, out tempIntValue))
                component.Fitting_No_Of_Holes_Required = tempIntValue;

            component.Fitting_Speed_Connect_YN =
               arg.Properties.FirstOrDefault(x =>
                   x.Name.Equals(nameof(TechnicalSpecification.Fitting_Speed_Connect_YN), StringComparison.OrdinalIgnoreCase))?.Value;

            component.Fitting_Spout_Height =
               arg.Properties.FirstOrDefault(x =>
                   x.Name.Equals(nameof(TechnicalSpecification.Fitting_Spout_Height), StringComparison.OrdinalIgnoreCase))?.Value;

            component.Fitting_Spout_Reach =
               arg.Properties.FirstOrDefault(x =>
                   x.Name.Equals(nameof(TechnicalSpecification.Fitting_Spout_Reach), StringComparison.OrdinalIgnoreCase))?.Value;

            component.Tub_Depth =
               arg.Properties.FirstOrDefault(x =>
                   x.Name.Equals(nameof(TechnicalSpecification.Tub_Depth), StringComparison.OrdinalIgnoreCase))?.Value;

            component.Tub_Overflow_Height =
               arg.Properties.FirstOrDefault(x =>
                   x.Name.Equals(nameof(TechnicalSpecification.Tub_Overflow_Height), StringComparison.OrdinalIgnoreCase))?.Value;

            component.Tub_Water_Depth =
               arg.Properties.FirstOrDefault(x =>
                   x.Name.Equals(nameof(TechnicalSpecification.Tub_Water_Depth), StringComparison.OrdinalIgnoreCase))?.Value;


            viewProperty = arg.Properties.FirstOrDefault(x => x.Name.Equals(nameof(TechnicalSpecification.Fitting_Swivel_Degrees), StringComparison.OrdinalIgnoreCase));
            if (!string.IsNullOrEmpty(viewProperty?.Value) && int.TryParse(viewProperty.Value, out tempIntValue))
                component.Fitting_Swivel_Degrees = tempIntValue;


            component.Fitting_Valve_Needed_For_Install_YN =
              arg.Properties.FirstOrDefault(x =>
                  x.Name.Equals(nameof(TechnicalSpecification.Fitting_Valve_Needed_For_Install_YN), StringComparison.OrdinalIgnoreCase))?.Value;

            component.FlowRateGPC =
                arg.Properties.FirstOrDefault(x =>
                    x.Name.Equals(nameof(TechnicalSpecification.FlowRateGPC), StringComparison.OrdinalIgnoreCase))?.Value;


            viewProperty = arg.Properties.FirstOrDefault(x => x.Name.Equals(nameof(TechnicalSpecification.Handle_Length), StringComparison.OrdinalIgnoreCase));
            if (!string.IsNullOrEmpty(viewProperty?.Value) && decimal.TryParse(viewProperty.Value, out tempDoubleValue))
                component.Handle_Length = tempDoubleValue;

            viewProperty = arg.Properties.FirstOrDefault(x => x.Name.Equals(nameof(TechnicalSpecification.Operating_Pressure_Static), StringComparison.OrdinalIgnoreCase));
            if (!string.IsNullOrEmpty(viewProperty?.Value) && int.TryParse(viewProperty.Value, out tempIntValue))
                component.Operating_Pressure_Static = tempIntValue;


            component.Handle_To_Handle_Measurement =
                arg.Properties.FirstOrDefault(x =>
                    x.Name.Equals(nameof(TechnicalSpecification.Handle_To_Handle_Measurement), StringComparison.OrdinalIgnoreCase))?.Value;


            component.Fitting_Valve_Material =
                arg.Properties.FirstOrDefault(x =>
                    x.Name.Equals(nameof(TechnicalSpecification.Fitting_Valve_Material), StringComparison.OrdinalIgnoreCase))?.Value;

            component.Hose_Included_YN =
               arg.Properties.FirstOrDefault(x =>
                   x.Name.Equals(nameof(TechnicalSpecification.Hose_Included_YN), StringComparison.OrdinalIgnoreCase))?.Value;


            component.Input_Connection_Type =
                arg.Properties.FirstOrDefault(x =>
                    x.Name.Equals(nameof(TechnicalSpecification.Input_Connection_Type), StringComparison.OrdinalIgnoreCase))?.Value;

            component.Installation_Hardware_Included =
                arg.Properties.FirstOrDefault(x =>
                    x.Name.Equals(nameof(TechnicalSpecification.Installation_Hardware_Included), StringComparison.OrdinalIgnoreCase))?.Value;


            component.Tub_Wattage =
                arg.Properties.FirstOrDefault(x =>
                    x.Name.Equals(nameof(TechnicalSpecification.Tub_Wattage), StringComparison.OrdinalIgnoreCase))?.Value;


            component.Tub_Blower_Placement =
              arg.Properties.FirstOrDefault(x =>
                  x.Name.Equals(nameof(TechnicalSpecification.Tub_Blower_Placement), StringComparison.OrdinalIgnoreCase))?.Value;

            component.Tub_Pump_Placement =
                arg.Properties.FirstOrDefault(x =>
                    x.Name.Equals(nameof(TechnicalSpecification.Tub_Pump_Placement), StringComparison.OrdinalIgnoreCase))?.Value;

            viewProperty = arg.Properties.FirstOrDefault(x => x.Name.Equals(nameof(TechnicalSpecification.Tub_Pump_Speeds), StringComparison.OrdinalIgnoreCase));
            if (!string.IsNullOrEmpty(viewProperty?.Value) && int.TryParse(viewProperty.Value, out tempIntValue))
                component.Tub_Pump_Speeds = tempIntValue;


            component.Tub_Sound_Dampening =
               arg.Properties.FirstOrDefault(x =>
                   x.Name.Equals(nameof(TechnicalSpecification.Tub_Sound_Dampening), StringComparison.OrdinalIgnoreCase))?.Value;


            component.Tank_Included_YN =
                arg.Properties.FirstOrDefault(x =>
                    x.Name.Equals(nameof(TechnicalSpecification.Tank_Included_YN), StringComparison.OrdinalIgnoreCase))?.Value;

            component.Sink_Predrilled_Holes_YN =
               arg.Properties.FirstOrDefault(x =>
                   x.Name.Equals(nameof(TechnicalSpecification.Sink_Predrilled_Holes_YN), StringComparison.OrdinalIgnoreCase))?.Value;



            component.Number_Of_Toilet_Trapways =
                arg.Properties.FirstOrDefault(x =>
                    x.Name.Equals(nameof(TechnicalSpecification.Number_Of_Toilet_Trapways), StringComparison.OrdinalIgnoreCase))?.Value;

            viewProperty = arg.Properties.FirstOrDefault(x => x.Name.Equals(nameof(TechnicalSpecification.Number_Of_Toilet_Flush_Valves), StringComparison.OrdinalIgnoreCase));
            if (!string.IsNullOrEmpty(viewProperty?.Value) && int.TryParse(viewProperty.Value, out tempIntValue))
                component.Number_Of_Toilet_Flush_Valves = tempIntValue;

            component.Mold =
                arg.Properties.FirstOrDefault(x =>
                    x.Name.Equals(nameof(TechnicalSpecification.Mold), StringComparison.OrdinalIgnoreCase))?.Value;

            viewProperty = arg.Properties.FirstOrDefault(x => x.Name.Equals(nameof(TechnicalSpecification.Bidet_No_Of_Dryer_Settings), StringComparison.OrdinalIgnoreCase));
            if (!string.IsNullOrEmpty(viewProperty?.Value) && int.TryParse(viewProperty.Value, out tempIntValue))
                component.Bidet_No_Of_Dryer_Settings = tempIntValue;

            viewProperty = arg.Properties.FirstOrDefault(x => x.Name.Equals(nameof(TechnicalSpecification.Bidet_No_Of_Spray_Nozzles), StringComparison.OrdinalIgnoreCase));
            if (!string.IsNullOrEmpty(viewProperty?.Value) && int.TryParse(viewProperty.Value, out tempIntValue))
                component.Bidet_No_Of_Spray_Nozzles = tempIntValue;

            viewProperty = arg.Properties.FirstOrDefault(x => x.Name.Equals(nameof(TechnicalSpecification.Bidet_No_Of_Water_Control_Settings), StringComparison.OrdinalIgnoreCase));
            if (!string.IsNullOrEmpty(viewProperty?.Value) && int.TryParse(viewProperty.Value, out tempIntValue))
                component.Bidet_No_Of_Water_Control_Settings = tempIntValue;

            component.Bath_Fitting_Bidet_Spray_Type =
                arg.Properties.FirstOrDefault(x =>
                    x.Name.Equals(nameof(TechnicalSpecification.Bath_Fitting_Bidet_Spray_Type), StringComparison.OrdinalIgnoreCase))?.Value;

            viewProperty = arg.Properties.FirstOrDefault(x => x.Name.Equals(nameof(TechnicalSpecification.Power_Cord_Length), StringComparison.OrdinalIgnoreCase));
            if (!string.IsNullOrEmpty(viewProperty?.Value) && int.TryParse(viewProperty.Value, out tempIntValue))
                component.Power_Cord_Length = tempIntValue;

            component.Toilet_Seat_Front_Type =
                arg.Properties.FirstOrDefault(x =>
                    x.Name.Equals(nameof(TechnicalSpecification.Toilet_Seat_Front_Type), StringComparison.OrdinalIgnoreCase))?.Value;

            component.Flushing_Mechanism =
                arg.Properties.FirstOrDefault(x =>
                    x.Name.Equals(nameof(TechnicalSpecification.Flushing_Mechanism), StringComparison.OrdinalIgnoreCase))?.Value;

            component.Bidet_Remote_Included_YN =
         arg.Properties.FirstOrDefault(x =>
             x.Name.Equals(nameof(TechnicalSpecification.Bidet_Remote_Included_YN), StringComparison.OrdinalIgnoreCase))?.Value;

            component.Vanity_Assembled_Depth =
               arg.Properties.FirstOrDefault(x =>
                   x.Name.Equals(nameof(TechnicalSpecification.Vanity_Assembled_Depth), StringComparison.OrdinalIgnoreCase))?.Value;

            component.Vanity_Cabinet_Height =
              arg.Properties.FirstOrDefault(x =>
                  x.Name.Equals(nameof(TechnicalSpecification.Vanity_Cabinet_Height), StringComparison.OrdinalIgnoreCase))?.Value;


            component.Vanity_Cabinet_Width =
               arg.Properties.FirstOrDefault(x =>
                   x.Name.Equals(nameof(TechnicalSpecification.Vanity_Cabinet_Width), StringComparison.OrdinalIgnoreCase))?.Value;

            component.Vanity_Assembled_Depth =
               arg.Properties.FirstOrDefault(x =>
                   x.Name.Equals(nameof(TechnicalSpecification.Vanity_Assembled_Depth), StringComparison.OrdinalIgnoreCase))?.Value;


            component.No_Of_Batteries_Included =
                arg.Properties.FirstOrDefault(x =>
                    x.Name.Equals(nameof(TechnicalSpecification.No_Of_Batteries_Included), StringComparison.OrdinalIgnoreCase))?.Value;


            component.Tub_Drain_Opening_Size =
               arg.Properties.FirstOrDefault(x =>
                   x.Name.Equals(nameof(TechnicalSpecification.Tub_Drain_Opening_Size), StringComparison.OrdinalIgnoreCase))?.Value;


            component.Tub_Shower_Base_Threshold_Height =
               arg.Properties.FirstOrDefault(x =>
                   x.Name.Equals(nameof(TechnicalSpecification.Tub_Shower_Base_Threshold_Height), StringComparison.OrdinalIgnoreCase))?.Value;


            viewProperty = arg.Properties.FirstOrDefault(x => x.Name.Equals(nameof(TechnicalSpecification.Shower_Base_No_Finished_Sides), StringComparison.OrdinalIgnoreCase));
            if (!string.IsNullOrEmpty(viewProperty?.Value) && int.TryParse(viewProperty.Value, out tempIntValue))
                component.Shower_Base_No_Finished_Sides = tempIntValue;

            viewProperty = arg.Properties.FirstOrDefault(x => x.Name.Equals(nameof(TechnicalSpecification.Shower_Base_Number_Of_Curbs), StringComparison.OrdinalIgnoreCase));
            if (!string.IsNullOrEmpty(viewProperty?.Value) && int.TryParse(viewProperty.Value, out tempIntValue))
                component.Shower_Base_Number_Of_Curbs = tempIntValue;

            component.Door_Glass_Thickness =
                arg.Properties.FirstOrDefault(x =>
                    x.Name.Equals(nameof(TechnicalSpecification.Door_Glass_Thickness), StringComparison.OrdinalIgnoreCase))?.Value;

            component.Door_Hinged_YN =
              arg.Properties.FirstOrDefault(x =>
                  x.Name.Equals(nameof(TechnicalSpecification.Door_Hinged_YN), StringComparison.OrdinalIgnoreCase))?.Value;


            component.Toilet_Seat_Width_Inside =
                arg.Properties.FirstOrDefault(x =>
                    x.Name.Equals(nameof(TechnicalSpecification.Toilet_Seat_Width_Inside), StringComparison.OrdinalIgnoreCase))?.Value;

            component.Toilet_Seat_Length_Inside =
               arg.Properties.FirstOrDefault(x =>
                   x.Name.Equals(nameof(TechnicalSpecification.Toilet_Seat_Length_Inside), StringComparison.OrdinalIgnoreCase))?.Value;


            component.Door_Usage =
                arg.Properties.FirstOrDefault(x =>
                x.Name.Equals(nameof(TechnicalSpecification.Door_Usage), StringComparison.OrdinalIgnoreCase))?.Value;

            component.Drain_Location =
                arg.Properties.FirstOrDefault(x =>
                x.Name.Equals(nameof(TechnicalSpecification.Drain_Location), StringComparison.OrdinalIgnoreCase))?.Value;

            component.Towel_Bar_Included_YN =
               arg.Properties.FirstOrDefault(x =>
                   x.Name.Equals(nameof(TechnicalSpecification.Towel_Bar_Included_YN), StringComparison.OrdinalIgnoreCase))?.Value;

            viewProperty = arg.Properties.FirstOrDefault(x => x.Name.Equals(nameof(TechnicalSpecification.Tub_Shower_Door_Width), StringComparison.OrdinalIgnoreCase));
            if (!string.IsNullOrEmpty(viewProperty?.Value) && decimal.TryParse(viewProperty.Value, out tempDoubleValue))
                component.Tub_Shower_Door_Width = tempDoubleValue;

            component.Shower_Wall_Caulkless_YN =
               arg.Properties.FirstOrDefault(x =>
                   x.Name.Equals(nameof(TechnicalSpecification.Shower_Wall_Caulkless_YN), StringComparison.OrdinalIgnoreCase))?.Value;


            component.Shower_Wall_Construction =
                arg.Properties.FirstOrDefault(x =>
                    x.Name.Equals(nameof(TechnicalSpecification.Shower_Wall_Construction), StringComparison.OrdinalIgnoreCase))?.Value;

            component.Shower_Wall_Surface =
                arg.Properties.FirstOrDefault(x =>
                    x.Name.Equals(nameof(TechnicalSpecification.Shower_Wall_Surface), StringComparison.OrdinalIgnoreCase))?.Value;

            viewProperty = arg.Properties.FirstOrDefault(x => x.Name.Equals(nameof(TechnicalSpecification.Bowl_Right_Side_Below_Counter_Depth),
                StringComparison.OrdinalIgnoreCase));
            if (!string.IsNullOrEmpty(viewProperty?.Value) && decimal.TryParse(viewProperty.Value, out tempDoubleValue))
                component.Bowl_Right_Side_Below_Counter_Depth = tempDoubleValue;

            viewProperty = arg.Properties.FirstOrDefault(x => x.Name.Equals(nameof(TechnicalSpecification.Sink_Cut_Out_Below_Counter_Depth), StringComparison.OrdinalIgnoreCase));
            if (!string.IsNullOrEmpty(viewProperty?.Value) && decimal.TryParse(viewProperty.Value, out tempDoubleValue))
                component.Sink_Cut_Out_Below_Counter_Depth = tempDoubleValue;

            viewProperty = arg.Properties.FirstOrDefault(x => x.Name.Equals(nameof(TechnicalSpecification.Sink_Cut_Out_Front_To_Back_Width), StringComparison.OrdinalIgnoreCase));
            if (!string.IsNullOrEmpty(viewProperty?.Value) && decimal.TryParse(viewProperty.Value, out tempDoubleValue))
                component.Sink_Cut_Out_Front_To_Back_Width = tempDoubleValue;

            viewProperty = arg.Properties.FirstOrDefault(x => x.Name.Equals(nameof(TechnicalSpecification.Sink_Cut_Out_Left_To_Right_Length), StringComparison.OrdinalIgnoreCase));
            if (!string.IsNullOrEmpty(viewProperty?.Value) && decimal.TryParse(viewProperty.Value, out tempDoubleValue))
                component.Sink_Cut_Out_Left_To_Right_Length = tempDoubleValue;

            viewProperty = arg.Properties.FirstOrDefault(x => x.Name.Equals(nameof(TechnicalSpecification.Sink_Depth_To_Overflow), StringComparison.OrdinalIgnoreCase));
            if (!string.IsNullOrEmpty(viewProperty?.Value) && decimal.TryParse(viewProperty.Value, out tempDoubleValue))
                component.Sink_Depth_To_Overflow = tempDoubleValue;

            viewProperty = arg.Properties.FirstOrDefault(x => x.Name.Equals(nameof(TechnicalSpecification.Sink_Minimum_Cabinet_Size), StringComparison.OrdinalIgnoreCase));
            if (!string.IsNullOrEmpty(viewProperty?.Value) && int.TryParse(viewProperty.Value, out tempIntValue))
                component.Sink_Minimum_Cabinet_Size = tempIntValue;

            component.Sink_Overflow_Location = arg.Properties.FirstOrDefault(x => x.Name.Equals(nameof(TechnicalSpecification.Sink_Overflow_Location), StringComparison.OrdinalIgnoreCase))?.Value;

            viewProperty = arg.Properties.FirstOrDefault(x => x.Name.Equals(nameof(TechnicalSpecification.Soaking_Depth), StringComparison.OrdinalIgnoreCase));
            if (!string.IsNullOrEmpty(viewProperty?.Value) && decimal.TryParse(viewProperty.Value, out tempDoubleValue))
                component.Soaking_Depth = tempDoubleValue;

            component.Tub_Blower =
                arg.Properties.FirstOrDefault(x =>
                    x.Name.Equals(nameof(TechnicalSpecification.Tub_Blower), StringComparison.OrdinalIgnoreCase))?.Value;

            viewProperty = arg.Properties.FirstOrDefault(x => x.Name.Equals(nameof(TechnicalSpecification.Tub_Pump_Amperage), StringComparison.OrdinalIgnoreCase));
            if (!string.IsNullOrEmpty(viewProperty?.Value) && int.TryParse(viewProperty.Value, out tempIntValue))
                component.Tub_Pump_Amperage = tempIntValue;

            viewProperty = arg.Properties.FirstOrDefault(x => x.Name.Equals(nameof(TechnicalSpecification.Tub_Water_Capacity), StringComparison.OrdinalIgnoreCase));
            if (!string.IsNullOrEmpty(viewProperty?.Value) && int.TryParse(viewProperty.Value, out tempIntValue))
                component.Tub_Water_Capacity = tempIntValue;

            viewProperty = arg.Properties.FirstOrDefault(x => x.Name.Equals(nameof(TechnicalSpecification.Sink_Bowl_Below_Counter_Depth), StringComparison.OrdinalIgnoreCase));
            if (!string.IsNullOrEmpty(viewProperty?.Value) && decimal.TryParse(viewProperty.Value, out tempDoubleValue))
                component.Sink_Bowl_Below_Counter_Depth = tempDoubleValue;

            viewProperty = arg.Properties.FirstOrDefault(x => x.Name.Equals(nameof(TechnicalSpecification.Sink_Front_To_Back_Width), StringComparison.OrdinalIgnoreCase));
            if (!string.IsNullOrEmpty(viewProperty?.Value) && decimal.TryParse(viewProperty.Value, out tempDoubleValue))
                component.Sink_Front_To_Back_Width = tempDoubleValue;

            viewProperty = arg.Properties.FirstOrDefault(x => x.Name.Equals(nameof(TechnicalSpecification.Sink_Left_To_Right_Length), StringComparison.OrdinalIgnoreCase));
            if (!string.IsNullOrEmpty(viewProperty?.Value) && decimal.TryParse(viewProperty.Value, out tempDoubleValue))
                component.Sink_Left_To_Right_Length = tempDoubleValue;

            viewProperty = arg.Properties.FirstOrDefault(x => x.Name.Equals(nameof(TechnicalSpecification.Sink_Top_To_Bottom_Depth), StringComparison.OrdinalIgnoreCase));
            if (!string.IsNullOrEmpty(viewProperty?.Value) && decimal.TryParse(viewProperty.Value, out tempDoubleValue))
                component.Sink_Top_To_Bottom_Depth = tempDoubleValue;


            viewProperty = arg.Properties.FirstOrDefault(x => x.Name.Equals(nameof(TechnicalSpecification.Sump_Length), StringComparison.OrdinalIgnoreCase));
            if (!string.IsNullOrEmpty(viewProperty?.Value) && decimal.TryParse(viewProperty.Value, out tempDoubleValue))
                component.Sump_Length = tempDoubleValue;

            viewProperty = arg.Properties.FirstOrDefault(x => x.Name.Equals(nameof(TechnicalSpecification.Sump_Width), StringComparison.OrdinalIgnoreCase));
            if (!string.IsNullOrEmpty(viewProperty?.Value) && decimal.TryParse(viewProperty.Value, out tempDoubleValue))
                component.Sump_Width = tempDoubleValue;

            component.Flow_Rate =
                arg.Properties.FirstOrDefault(x =>
                    x.Name.Equals(nameof(TechnicalSpecification.Flow_Rate), StringComparison.OrdinalIgnoreCase))?.Value;

            component.Flush_Technology =
                arg.Properties.FirstOrDefault(x =>
                    x.Name.Equals(nameof(TechnicalSpecification.Flush_Technology), StringComparison.OrdinalIgnoreCase))?.Value;


            component.Sink_Bowl_Split =
               arg.Properties.FirstOrDefault(x =>
                   x.Name.Equals(nameof(TechnicalSpecification.Sink_Bowl_Split), StringComparison.OrdinalIgnoreCase))?.Value;

            viewProperty = arg.Properties.FirstOrDefault(x => x.Name.Equals(nameof(TechnicalSpecification.MAP_Performance_Rating), StringComparison.OrdinalIgnoreCase));
            if (!string.IsNullOrEmpty(viewProperty?.Value) && int.TryParse(viewProperty.Value, out tempIntValue))
                component.MAP_Performance_Rating = tempIntValue;

            viewProperty = arg.Properties.FirstOrDefault(x => x.Name.Equals(nameof(TechnicalSpecification.Bowl_Height_Without_Seat), StringComparison.OrdinalIgnoreCase));
            if (!string.IsNullOrEmpty(viewProperty?.Value) && decimal.TryParse(viewProperty.Value, out tempDoubleValue))
                component.Bowl_Height_Without_Seat = tempDoubleValue;


            viewProperty = arg.Properties.FirstOrDefault(x => x.Name.Equals(nameof(TechnicalSpecification.Tub_Shower_Door_Height), StringComparison.OrdinalIgnoreCase));
            if (!string.IsNullOrEmpty(viewProperty?.Value) && decimal.TryParse(viewProperty.Value, out tempDoubleValue))
                component.Tub_Shower_Door_Height = tempDoubleValue;

            viewProperty = arg.Properties.FirstOrDefault(x => x.Name.Equals(nameof(TechnicalSpecification.Tub_Shower_Door_Opening_Width_Max), StringComparison.OrdinalIgnoreCase));
            if (!string.IsNullOrEmpty(viewProperty?.Value) && int.TryParse(viewProperty.Value, out tempIntValue))
                component.Tub_Shower_Door_Opening_Width_Max = tempIntValue;

            viewProperty = arg.Properties.FirstOrDefault(x => x.Name.Equals(nameof(TechnicalSpecification.Tub_Shower_Door_Opening_Width_Min), StringComparison.OrdinalIgnoreCase));
            if (!string.IsNullOrEmpty(viewProperty?.Value) && decimal.TryParse(viewProperty.Value, out tempDoubleValue))
                component.Tub_Shower_Door_Opening_Width_Min = tempDoubleValue;

            viewProperty = arg.Properties.FirstOrDefault(x => x.Name.Equals(nameof(TechnicalSpecification.Flush_Valve_Size), StringComparison.OrdinalIgnoreCase));
            if (!string.IsNullOrEmpty(viewProperty?.Value) && decimal.TryParse(viewProperty.Value, out tempDoubleValue))
                component.Flush_Valve_Size = tempDoubleValue;

            viewProperty = arg.Properties.FirstOrDefault(x => x.Name.Equals(nameof(TechnicalSpecification.Operating_Pressure_Flushing), StringComparison.OrdinalIgnoreCase));
            if (!string.IsNullOrEmpty(viewProperty?.Value) && int.TryParse(viewProperty.Value, out tempIntValue))
                component.Operating_Pressure_Flushing = tempIntValue;


            viewProperty = arg.Properties.FirstOrDefault(x => x.Name.Equals(nameof(TechnicalSpecification.Rough_In_Size), StringComparison.OrdinalIgnoreCase));
            if (!string.IsNullOrEmpty(viewProperty?.Value) && decimal.TryParse(viewProperty.Value, out tempDoubleValue))
                component.Rough_In_Size = tempDoubleValue;

            viewProperty = arg.Properties.FirstOrDefault(x => x.Name.Equals(nameof(TechnicalSpecification.Toilet_Bowl_Height_With_Seat), StringComparison.OrdinalIgnoreCase));
            if (!string.IsNullOrEmpty(viewProperty?.Value) && int.TryParse(viewProperty.Value, out tempIntValue))
                component.Toilet_Bowl_Height_With_Seat = tempIntValue;

            viewProperty = arg.Properties.FirstOrDefault(x => x.Name.Equals(nameof(TechnicalSpecification.Toilet_Trapway_Diameter), StringComparison.OrdinalIgnoreCase));
            if (!string.IsNullOrEmpty(viewProperty?.Value) && int.TryParse(viewProperty.Value, out tempIntValue))
                component.Toilet_Trapway_Diameter = tempIntValue;

            viewProperty = arg.Properties.FirstOrDefault(x => x.Name.Equals(nameof(TechnicalSpecification.Bidet_No_Of_Heat_Settings), StringComparison.OrdinalIgnoreCase));
            if (!string.IsNullOrEmpty(viewProperty?.Value) && int.TryParse(viewProperty.Value, out tempIntValue))
                component.Bidet_No_Of_Heat_Settings = tempIntValue;

            component.Battery_Weight =
                arg.Properties.FirstOrDefault(x =>
                    x.Name.Equals(nameof(TechnicalSpecification.Battery_Weight), StringComparison.OrdinalIgnoreCase))?.Value;

            component.Chemical_Composition_Of_Battery =
                arg.Properties.FirstOrDefault(x =>
                    x.Name.Equals(nameof(TechnicalSpecification.Chemical_Composition_Of_Battery), StringComparison.OrdinalIgnoreCase))?.Value;

            viewProperty = arg.Properties.FirstOrDefault(x => x.Name.Equals(nameof(TechnicalSpecification.Toilet_Seat_Bolt_Spread), StringComparison.OrdinalIgnoreCase));
            if (!string.IsNullOrEmpty(viewProperty?.Value) && int.TryParse(viewProperty.Value, out tempIntValue))
                component.Toilet_Seat_Bolt_Spread = tempIntValue;

            viewProperty = arg.Properties.FirstOrDefault(x => x.Name.Equals(nameof(TechnicalSpecification.Escutcheon_Internal_Diameter), StringComparison.OrdinalIgnoreCase));
            if (!string.IsNullOrEmpty(viewProperty?.Value) && int.TryParse(viewProperty.Value, out tempIntValue))
                component.Escutcheon_Internal_Diameter = tempIntValue;

            viewProperty = arg.Properties.FirstOrDefault(x => x.Name.Equals(nameof(TechnicalSpecification.Number_Input_Valves_Connection), StringComparison.OrdinalIgnoreCase));
            if (!string.IsNullOrEmpty(viewProperty?.Value) && int.TryParse(viewProperty.Value, out tempIntValue))
                component.Number_Input_Valves_Connection = tempIntValue;

            viewProperty = arg.Properties.FirstOrDefault(x => x.Name.Equals(nameof(TechnicalSpecification.Bath_Accessory_Weight_Capacity), StringComparison.OrdinalIgnoreCase));
            if (!string.IsNullOrEmpty(viewProperty?.Value) && int.TryParse(viewProperty.Value, out tempIntValue))
                component.Bath_Accessory_Weight_Capacity = tempIntValue;

            viewProperty = arg.Properties.FirstOrDefault(x => x.Name.Equals(nameof(TechnicalSpecification.Bristle_Stiffness), StringComparison.OrdinalIgnoreCase));
            if (!string.IsNullOrEmpty(viewProperty?.Value) && int.TryParse(viewProperty.Value, out tempIntValue))
                component.Bristle_Stiffness = tempIntValue;

            viewProperty = arg.Properties.FirstOrDefault(x => x.Name.Equals(nameof(TechnicalSpecification.Bristle_Length), StringComparison.OrdinalIgnoreCase));
            if (!string.IsNullOrEmpty(viewProperty?.Value) && int.TryParse(viewProperty.Value, out tempIntValue))
                component.Bristle_Length = tempIntValue;

            component.Shower_Arm_Reach =
                arg.Properties.FirstOrDefault(x =>
                    x.Name.Equals(nameof(TechnicalSpecification.Shower_Arm_Reach), StringComparison.OrdinalIgnoreCase))?.Value;

            viewProperty = arg.Properties.FirstOrDefault(x => x.Name.Equals(nameof(TechnicalSpecification.Shower_Wall_Bar_Length), StringComparison.OrdinalIgnoreCase));
            if (!string.IsNullOrEmpty(viewProperty?.Value) && int.TryParse(viewProperty.Value, out tempIntValue))
                component.Shower_Wall_Bar_Length = tempIntValue;


            component.Rough_In_Valve_Features =
               arg.Properties.FirstOrDefault(x =>
                   x.Name.Equals(nameof(TechnicalSpecification.Rough_In_Valve_Features), StringComparison.OrdinalIgnoreCase))?.Value;


            component.Concealed_Or_Exposed_Flush_Valve =
                arg.Properties.FirstOrDefault(x =>
                    x.Name.Equals(nameof(TechnicalSpecification.Concealed_Or_Exposed_Flush_Valve), StringComparison.OrdinalIgnoreCase))?.Value;

            component.Assembly_Required_YN =
               arg.Properties.FirstOrDefault(x =>
                   x.Name.Equals(nameof(TechnicalSpecification.Assembly_Required_YN), StringComparison.OrdinalIgnoreCase))?.Value;

            component.Bath_Accessory_Installation_Type =
               arg.Properties.FirstOrDefault(x =>
                   x.Name.Equals(nameof(TechnicalSpecification.Bath_Accessory_Installation_Type), StringComparison.OrdinalIgnoreCase))?.Value;

            viewProperty = arg.Properties.FirstOrDefault(x => x.Name.Equals(nameof(TechnicalSpecification.Bath_Shower_Diameter), StringComparison.OrdinalIgnoreCase));
            if (!string.IsNullOrEmpty(viewProperty?.Value) && int.TryParse(viewProperty.Value, out tempIntValue))
                component.Bath_Shower_Diameter = tempIntValue;

            viewProperty = arg.Properties.FirstOrDefault(x => x.Name.Equals(nameof(TechnicalSpecification.Bath_Shower_Maximum_Deck_Thickness), StringComparison.OrdinalIgnoreCase));
            if (!string.IsNullOrEmpty(viewProperty?.Value) && decimal.TryParse(viewProperty.Value, out tempDoubleValue))
                component.Bath_Shower_Maximum_Deck_Thickness = tempDoubleValue;

            viewProperty = arg.Properties.FirstOrDefault(x => x.Name.Equals(nameof(TechnicalSpecification.Bath_Shower_Showerhead_Face_Diameter), StringComparison.OrdinalIgnoreCase));
            if (!string.IsNullOrEmpty(viewProperty?.Value) && int.TryParse(viewProperty.Value, out tempIntValue))
                component.Bath_Shower_Showerhead_Face_Diameter = tempIntValue;

            viewProperty = arg.Properties.FirstOrDefault(x => x.Name.Equals(nameof(TechnicalSpecification.Bath_Shower_Sprayer_Face_Diameter), StringComparison.OrdinalIgnoreCase));
            if (!string.IsNullOrEmpty(viewProperty?.Value) && int.TryParse(viewProperty.Value, out tempIntValue))
                component.Bath_Shower_Sprayer_Face_Diameter = tempIntValue;

            component.Batteries_Included_YN =
                arg.Properties.FirstOrDefault(x =>
                    x.Name.Equals(nameof(TechnicalSpecification.Batteries_Included_YN), StringComparison.OrdinalIgnoreCase))?.Value;


            component.Compliant_Product_Substitute =
                arg.Properties.FirstOrDefault(x =>
                    x.Name.Equals(nameof(TechnicalSpecification.Compliant_Product_Substitute), StringComparison.OrdinalIgnoreCase))?.Value;

            viewProperty = arg.Properties.FirstOrDefault(x => x.Name.Equals(nameof(TechnicalSpecification.EA_Each_Weight), StringComparison.OrdinalIgnoreCase));
            if (!string.IsNullOrEmpty(viewProperty?.Value) && decimal.TryParse(viewProperty.Value, out tempDoubleValue))
                component.EA_Each_Weight = tempDoubleValue;

            viewProperty = arg.Properties.FirstOrDefault(x => x.Name.Equals(nameof(TechnicalSpecification.Faucet_Hole_Spacing), StringComparison.OrdinalIgnoreCase));
            if (!string.IsNullOrEmpty(viewProperty?.Value) && int.TryParse(viewProperty.Value, out tempIntValue))
                component.Faucet_Hole_Spacing = tempIntValue;


            component.Shower_Wall_Bar_Installation =
               arg.Properties.FirstOrDefault(x =>
                   x.Name.Equals(nameof(TechnicalSpecification.Shower_Wall_Bar_Installation), StringComparison.OrdinalIgnoreCase))?.Value;

            component.Tub_Interior_Shape =
                arg.Properties.FirstOrDefault(x =>
                    x.Name.Equals(nameof(TechnicalSpecification.Tub_Interior_Shape), StringComparison.OrdinalIgnoreCase))?.Value;

            viewProperty = arg.Properties.FirstOrDefault(x => x.Name.Equals(nameof(TechnicalSpecification.Tub_Voltage), StringComparison.OrdinalIgnoreCase));
            if (!string.IsNullOrEmpty(viewProperty?.Value) && int.TryParse(viewProperty.Value, out tempIntValue))
                component.Tub_Voltage = tempIntValue;

            component.Valve_Application_YN =
           arg.Properties.FirstOrDefault(x =>
               x.Name.Equals(nameof(TechnicalSpecification.Valve_Application_YN), StringComparison.OrdinalIgnoreCase))?.Value;



            component.Valve_Indoor_Outdoor =
                arg.Properties.FirstOrDefault(x =>
                    x.Name.Equals(nameof(TechnicalSpecification.Valve_Indoor_Outdoor), StringComparison.OrdinalIgnoreCase))?.Value;

            viewProperty = arg.Properties.FirstOrDefault(x => x.Name.Equals(nameof(TechnicalSpecification.Valve_Input_Diameter), StringComparison.OrdinalIgnoreCase));
            if (!string.IsNullOrEmpty(viewProperty?.Value) && int.TryParse(viewProperty.Value, out tempIntValue))
                component.Valve_Input_Diameter = tempIntValue;

            viewProperty = arg.Properties.FirstOrDefault(x => x.Name.Equals(nameof(TechnicalSpecification.Valve_Output_Diameter), StringComparison.OrdinalIgnoreCase));
            if (!string.IsNullOrEmpty(viewProperty?.Value) && int.TryParse(viewProperty.Value, out tempIntValue))
                component.Valve_Output_Diameter = tempIntValue;

            viewProperty = arg.Properties.FirstOrDefault(x => x.Name.Equals(nameof(TechnicalSpecification.Valves_Maximum_Pressure), StringComparison.OrdinalIgnoreCase));
            if (!string.IsNullOrEmpty(viewProperty?.Value) && int.TryParse(viewProperty.Value, out tempIntValue))
                component.Valves_Maximum_Pressure = tempIntValue;

            viewProperty = arg.Properties.FirstOrDefault(x => x.Name.Equals(nameof(TechnicalSpecification.Valves_Maximum_Working_Temperature), StringComparison.OrdinalIgnoreCase));
            if (!string.IsNullOrEmpty(viewProperty?.Value) && int.TryParse(viewProperty.Value, out tempIntValue))
                component.Valves_Maximum_Working_Temperature = tempIntValue;


            component.Volume_Control_YN =
          arg.Properties.FirstOrDefault(x =>
              x.Name.Equals(nameof(TechnicalSpecification.Volume_Control_YN), StringComparison.OrdinalIgnoreCase))?.Value;


            viewProperty = arg.Properties.FirstOrDefault(x => x.Name.Equals(nameof(TechnicalSpecification.Volume_SAP), StringComparison.OrdinalIgnoreCase));
            if (!string.IsNullOrEmpty(viewProperty?.Value) && decimal.TryParse(viewProperty.Value, out tempDoubleValue))
                component.Volume_SAP = tempDoubleValue;

            viewProperty = arg.Properties.FirstOrDefault(x => x.Name.Equals(nameof(TechnicalSpecification.Weight_Capacity), StringComparison.OrdinalIgnoreCase));
            if (!string.IsNullOrEmpty(viewProperty?.Value) && int.TryParse(viewProperty.Value, out tempIntValue))
                component.Weight_Capacity = tempIntValue;

            component.ADA_YN =
            arg.Properties.FirstOrDefault(x =>
                x.Name.Equals(nameof(TechnicalSpecification.ADA_YN), StringComparison.OrdinalIgnoreCase))?.Value;

            component.Barrier_Free =
            arg.Properties.FirstOrDefault(x =>
                x.Name.Equals(nameof(TechnicalSpecification.Barrier_Free), StringComparison.OrdinalIgnoreCase))?.Value;

            component.Massachusetts_Plumbing_Board_YN =
          arg.Properties.FirstOrDefault(x =>
              x.Name.Equals(nameof(TechnicalSpecification.Massachusetts_Plumbing_Board_YN), StringComparison.OrdinalIgnoreCase))?.Value;

            component.Certification_CAN_UL =
          arg.Properties.FirstOrDefault(x =>
              x.Name.Equals(nameof(TechnicalSpecification.Certification_CAN_UL), StringComparison.OrdinalIgnoreCase))?.Value;

            component.Smoke_And_Flame_Compliant =
          arg.Properties.FirstOrDefault(x =>
              x.Name.Equals(nameof(TechnicalSpecification.Smoke_And_Flame_Compliant), StringComparison.OrdinalIgnoreCase))?.Value;

            component.Smoke_And_Flame_Compliant =
          arg.Properties.FirstOrDefault(x =>
              x.Name.Equals(nameof(TechnicalSpecification.Smoke_And_Flame_Compliant), StringComparison.OrdinalIgnoreCase))?.Value;

            component.Proposition_65_Disclosure_YN =
              arg.Properties.FirstOrDefault(x =>
                  x.Name.Equals(nameof(TechnicalSpecification.Proposition_65_Disclosure_YN), StringComparison.OrdinalIgnoreCase))?.Value;

            component.EPA_Watersense_YN =
              arg.Properties.FirstOrDefault(x =>
                  x.Name.Equals(nameof(TechnicalSpecification.EPA_Watersense_YN), StringComparison.OrdinalIgnoreCase))?.Value;

            component.Certification_UL =
              arg.Properties.FirstOrDefault(x =>
                  x.Name.Equals(nameof(TechnicalSpecification.Certification_UL), StringComparison.OrdinalIgnoreCase))?.Value;

            component.CEC_Certified_YN =
             arg.Properties.FirstOrDefault(x =>
                 x.Name.Equals(nameof(TechnicalSpecification.CEC_Certified_YN), StringComparison.OrdinalIgnoreCase))?.Value;

            component.Certification_NSF =
            arg.Properties.FirstOrDefault(x =>
                x.Name.Equals(nameof(TechnicalSpecification.Certification_NSF), StringComparison.OrdinalIgnoreCase))?.Value;

            component.Cal_Green_YN =
            arg.Properties.FirstOrDefault(x =>
                x.Name.Equals(nameof(TechnicalSpecification.Cal_Green_YN), StringComparison.OrdinalIgnoreCase))?.Value;

            component.Vanity_Assembled_Height =
               arg.Properties.FirstOrDefault(x =>
                   x.Name.Equals(nameof(TechnicalSpecification.Vanity_Assembled_Height), StringComparison.OrdinalIgnoreCase))?.Value;

            component.Vanity_Assembled_Weight =
               arg.Properties.FirstOrDefault(x =>
                   x.Name.Equals(nameof(TechnicalSpecification.Vanity_Assembled_Weight), StringComparison.OrdinalIgnoreCase))?.Value;

            component.Product_Height =
              arg.Properties.FirstOrDefault(x =>
                  x.Name.Equals(nameof(TechnicalSpecification.Product_Height), StringComparison.OrdinalIgnoreCase))?.Value;

            component.Product_Weight =
               arg.Properties.FirstOrDefault(x =>
                   x.Name.Equals(nameof(TechnicalSpecification.Product_Weight), StringComparison.OrdinalIgnoreCase))?.Value;

            component.Product_Length =
               arg.Properties.FirstOrDefault(x =>
                   x.Name.Equals(nameof(TechnicalSpecification.Product_Length), StringComparison.OrdinalIgnoreCase))?.Value;

            component.Product_Width =
               arg.Properties.FirstOrDefault(x =>
                   x.Name.Equals(nameof(TechnicalSpecification.Product_Width), StringComparison.OrdinalIgnoreCase))?.Value;

            context.Logger.LogInformation("Current Entity Version : " + entity.Version);

            // Persist changes
            this.Commander.Pipeline<IPersistEntityPipeline>().Run(new PersistEntityArgument(entity), context);

            return Task.FromResult(arg);
        }
    }
}
