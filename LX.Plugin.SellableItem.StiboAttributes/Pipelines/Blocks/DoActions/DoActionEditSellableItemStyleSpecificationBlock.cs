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
    [PipelineDisplayName(SellableItemStiboAttributesConstants.Pipelines.Blocks.DoActionEditSellableItemStyleSpecificationBlock)]
    public class DoActionEditSellableItemStyleSpecificationBlock : PipelineBlock<EntityView, EntityView, CommercePipelineExecutionContext>
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
        public DoActionEditSellableItemStyleSpecificationBlock(CommerceCommander commander)
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


            var sellableItemStiboAttributesViewsPolicy = context.GetPolicy<StyleSpecificationActionsPolicy>();

            // Only proceed if the right action was invoked
            if (string.IsNullOrEmpty(arg.Action) || !arg.Action.Equals(sellableItemStiboAttributesViewsPolicy.StyleSpecification, StringComparison.OrdinalIgnoreCase))
            {
                return Task.FromResult(arg);
            }

            // Get the sellable item from the context
            var entity = context.CommerceContext.GetObject<Sitecore.Commerce.Plugin.Catalog.SellableItem>(x => x.Id.Equals(arg.EntityId));
            if (entity == null)
            {
                return Task.FromResult(arg);
            }

            StyleSpecification component;
            // Get the notes component from the sellable item or its variation
            if (!string.IsNullOrWhiteSpace(arg.ItemId))
            {
                component = entity.GetVariation(arg.ItemId).GetComponent<StyleSpecification>();
            }
            else
            {
                component = entity.GetComponent<StyleSpecification>();
            }

            int tempIntValue = 0;
            decimal tempDoubleValue = 0;

            component.Sink_Integral_Dish_Rack_YN =
                arg.Properties.FirstOrDefault(x =>
                    x.Name.Equals(nameof(StyleSpecification.Sink_Integral_Dish_Rack_YN), StringComparison.OrdinalIgnoreCase))?.Value;

            component.Drain_Included_YN =
                arg.Properties.FirstOrDefault(x =>
                    x.Name.Equals(nameof(StyleSpecification.Drain_Included_YN), StringComparison.OrdinalIgnoreCase))?.Value;

            component.Vanity_Hardware_Finish_Family =
                arg.Properties.FirstOrDefault(x =>
                    x.Name.Equals(nameof(StyleSpecification.Vanity_Hardware_Finish_Family), StringComparison.OrdinalIgnoreCase))?.Value;


            component.Drain_Material =
                arg.Properties.FirstOrDefault(x =>
                    x.Name.Equals(nameof(StyleSpecification.Drain_Material), StringComparison.OrdinalIgnoreCase))?.Value;

            ViewProperty viewProperty = arg.Properties.FirstOrDefault(x => x.Name.Equals(nameof(StyleSpecification.Base_Threshold),
                StringComparison.OrdinalIgnoreCase));
            if (!string.IsNullOrEmpty(viewProperty?.Value) && int.TryParse(viewProperty.Value, out tempIntValue))
                component.Base_Threshold = tempIntValue;

            component.Door_Glass_Finish =
                arg.Properties.FirstOrDefault(x =>
                    x.Name.Equals(nameof(StyleSpecification.Door_Glass_Finish), StringComparison.OrdinalIgnoreCase))?.Value;

            component.Bristle_Material =
                arg.Properties.FirstOrDefault(x =>
                    x.Name.Equals(nameof(StyleSpecification.Bristle_Material), StringComparison.OrdinalIgnoreCase))?.Value;


            component.Deck_Plate_Included_YN =
                arg.Properties.FirstOrDefault(x =>
                    x.Name.Equals(nameof(StyleSpecification.Deck_Plate_Included_YN), StringComparison.OrdinalIgnoreCase))?.Value;

            component.Shower_Trim_Type =
                arg.Properties.FirstOrDefault(x =>
                    x.Name.Equals(nameof(StyleSpecification.Shower_Trim_Type), StringComparison.OrdinalIgnoreCase))?.Value;


            component.Shower_Valve_Type =
                arg.Properties.FirstOrDefault(x =>
                    x.Name.Equals(nameof(StyleSpecification.Shower_Valve_Type), StringComparison.OrdinalIgnoreCase))?.Value;


            component.Shower_Wall_Installation_Type =
                arg.Properties.FirstOrDefault(x =>
                    x.Name.Equals(nameof(StyleSpecification.Shower_Wall_Installation_Type), StringComparison.OrdinalIgnoreCase))?.Value;


            component.Showerhead_Included_YN =
               arg.Properties.FirstOrDefault(x =>
                   x.Name.Equals(nameof(StyleSpecification.Showerhead_Included_YN), StringComparison.OrdinalIgnoreCase))?.Value;

            component.Side_Spray_YN =
              arg.Properties.FirstOrDefault(x =>
                  x.Name.Equals(nameof(StyleSpecification.Side_Spray_YN), StringComparison.OrdinalIgnoreCase))?.Value;

            component.Sink_Finish_Type =
                arg.Properties.FirstOrDefault(x =>
                    x.Name.Equals(nameof(StyleSpecification.Sink_Finish_Type), StringComparison.OrdinalIgnoreCase))?.Value;

            component.Sink_Shape =
               arg.Properties.FirstOrDefault(x =>
                   x.Name.Equals(nameof(StyleSpecification.Sink_Shape), StringComparison.OrdinalIgnoreCase))?.Value;

            component.Soap_Lotion_Dispenser_YN =
             arg.Properties.FirstOrDefault(x =>
                 x.Name.Equals(nameof(StyleSpecification.Soap_Lotion_Dispenser_YN), StringComparison.OrdinalIgnoreCase))?.Value;

            component.Sink_Rack_Type =
                arg.Properties.FirstOrDefault(x =>
                    x.Name.Equals(nameof(StyleSpecification.Sink_Rack_Type), StringComparison.OrdinalIgnoreCase))?.Value;

            component.Water_Filter_Included_YN =
               arg.Properties.FirstOrDefault(x =>
                   x.Name.Equals(nameof(StyleSpecification.Water_Filter_Included_YN), StringComparison.OrdinalIgnoreCase))?.Value;

            component.Spray_Pattern_List =
            arg.Properties.FirstOrDefault(x =>
                x.Name.Equals(nameof(StyleSpecification.Spray_Pattern_List), StringComparison.OrdinalIgnoreCase))?.Value;

            component.Mirror_Frame_Material =
              arg.Properties.FirstOrDefault(x =>
                  x.Name.Equals(nameof(StyleSpecification.Mirror_Frame_Material), StringComparison.OrdinalIgnoreCase))?.Value;

            component.Mirror_Framed_Or_Frameless =
                arg.Properties.FirstOrDefault(x =>
                    x.Name.Equals(nameof(StyleSpecification.Mirror_Framed_Or_Frameless), StringComparison.OrdinalIgnoreCase))?.Value;

            component.Mirror_Light_YN =
              arg.Properties.FirstOrDefault(x =>
                  x.Name.Equals(nameof(StyleSpecification.Mirror_Light_YN), StringComparison.OrdinalIgnoreCase))?.Value;

            component.Mirror_Magnifying_YN =
         arg.Properties.FirstOrDefault(x =>
             x.Name.Equals(nameof(StyleSpecification.Mirror_Magnifying_YN), StringComparison.OrdinalIgnoreCase))?.Value;

            component.Mirror_Shape =
                arg.Properties.FirstOrDefault(x =>
                    x.Name.Equals(nameof(StyleSpecification.Mirror_Shape), StringComparison.OrdinalIgnoreCase))?.Value;

            component.Mirror_Telescoping_YN =
               arg.Properties.FirstOrDefault(x =>
                   x.Name.Equals(nameof(StyleSpecification.Mirror_Telescoping_YN), StringComparison.OrdinalIgnoreCase))?.Value;

            component.Mirror_Type =
               arg.Properties.FirstOrDefault(x =>
                   x.Name.Equals(nameof(StyleSpecification.Mirror_Type), StringComparison.OrdinalIgnoreCase))?.Value;

            component.Product_Style =
                            arg.Properties.FirstOrDefault(x =>
                                x.Name.Equals(nameof(StyleSpecification.Product_Style), StringComparison.OrdinalIgnoreCase))?.Value;

            component.Pull_Down_YN =
          arg.Properties.FirstOrDefault(x =>
              x.Name.Equals(nameof(StyleSpecification.Pull_Down_YN), StringComparison.OrdinalIgnoreCase))?.Value;

            component.Shower_Arm_Style =
               arg.Properties.FirstOrDefault(x =>
                   x.Name.Equals(nameof(StyleSpecification.Shower_Arm_Style), StringComparison.OrdinalIgnoreCase))?.Value;

            component.Shower_Diverter_Type =
                arg.Properties.FirstOrDefault(x =>
                    x.Name.Equals(nameof(StyleSpecification.Shower_Diverter_Type), StringComparison.OrdinalIgnoreCase))?.Value;

            viewProperty = arg.Properties.FirstOrDefault(x => x.Name.Equals(nameof(StyleSpecification.Shower_No_Of_Spray_Settings), StringComparison.OrdinalIgnoreCase));
            if (!string.IsNullOrEmpty(viewProperty?.Value) && int.TryParse(viewProperty.Value, out tempIntValue))
                component.Shower_No_Of_Spray_Settings = tempIntValue;

            viewProperty = arg.Properties.FirstOrDefault(x => x.Name.Equals(nameof(StyleSpecification.Shower_No_Of_Jets), StringComparison.OrdinalIgnoreCase));
            if (!string.IsNullOrEmpty(viewProperty?.Value) && int.TryParse(viewProperty.Value, out tempIntValue))
                component.Shower_No_Of_Jets = tempIntValue;

            component.Fitting_Material =
               arg.Properties.FirstOrDefault(x =>
                   x.Name.Equals(nameof(StyleSpecification.Fitting_Material), StringComparison.OrdinalIgnoreCase))?.Value;

            component.Fitting_Mount_Type =
                arg.Properties.FirstOrDefault(x =>
                    x.Name.Equals(nameof(StyleSpecification.Fitting_Mount_Type), StringComparison.OrdinalIgnoreCase))?.Value;


            viewProperty = arg.Properties.FirstOrDefault(x => x.Name.Equals(nameof(StyleSpecification.Fitting_No_Of_Handles), StringComparison.OrdinalIgnoreCase));
            if (!string.IsNullOrEmpty(viewProperty?.Value) && int.TryParse(viewProperty.Value, out tempIntValue))
                component.Fitting_No_Of_Handles = tempIntValue;

            component.Fitting_Spout_Type =
                arg.Properties.FirstOrDefault(x =>
                    x.Name.Equals(nameof(StyleSpecification.Fitting_Spout_Type), StringComparison.OrdinalIgnoreCase))?.Value;


            component.Hinge_Material =
               arg.Properties.FirstOrDefault(x =>
                   x.Name.Equals(nameof(StyleSpecification.Hinge_Material), StringComparison.OrdinalIgnoreCase))?.Value;


            component.Fixture_Material =
                arg.Properties.FirstOrDefault(x =>
                    x.Name.Equals(nameof(StyleSpecification.Fixture_Material), StringComparison.OrdinalIgnoreCase))?.Value;


            component.Frame_Finish =
               arg.Properties.FirstOrDefault(x =>
                   x.Name.Equals(nameof(StyleSpecification.Frame_Finish), StringComparison.OrdinalIgnoreCase))?.Value;



            component.Hand_Head_Shower_Shape =
               arg.Properties.FirstOrDefault(x =>
                   x.Name.Equals(nameof(StyleSpecification.Hand_Head_Shower_Shape), StringComparison.OrdinalIgnoreCase))?.Value;

            component.Handle_Finish =
               arg.Properties.FirstOrDefault(x =>
                   x.Name.Equals(nameof(StyleSpecification.Handle_Finish), StringComparison.OrdinalIgnoreCase))?.Value;


            viewProperty = arg.Properties.FirstOrDefault(x => x.Name.Equals(nameof(StyleSpecification.Max_Occupants), StringComparison.OrdinalIgnoreCase));
            if (!string.IsNullOrEmpty(viewProperty?.Value) && int.TryParse(viewProperty.Value, out tempIntValue))
                component.Max_Occupants = tempIntValue;


            component.Flushing_Type =
               arg.Properties.FirstOrDefault(x =>
                   x.Name.Equals(nameof(StyleSpecification.Flushing_Type), StringComparison.OrdinalIgnoreCase))?.Value;

            component.Door_Swing =
               arg.Properties.FirstOrDefault(x =>
                   x.Name.Equals(nameof(StyleSpecification.Door_Swing), StringComparison.OrdinalIgnoreCase))?.Value;


            viewProperty = arg.Properties.FirstOrDefault(x => x.Name.Equals(nameof(StyleSpecification.Shower_Wall_Number_Of_Shelves), StringComparison.OrdinalIgnoreCase));
            if (!string.IsNullOrEmpty(viewProperty?.Value) && int.TryParse(viewProperty.Value, out tempIntValue))
                component.Shower_Wall_Number_Of_Shelves = tempIntValue;

            component.Handle_Style =
             arg.Properties.FirstOrDefault(x =>
                 x.Name.Equals(nameof(StyleSpecification.Handle_Style), StringComparison.OrdinalIgnoreCase))?.Value;


            component.Jet_Type =
                arg.Properties.FirstOrDefault(x =>
                    x.Name.Equals(nameof(StyleSpecification.Jet_Type), StringComparison.OrdinalIgnoreCase))?.Value;

            component.Tub_Lighting =
                arg.Properties.FirstOrDefault(x =>
                    x.Name.Equals(nameof(StyleSpecification.Tub_Lighting), StringComparison.OrdinalIgnoreCase))?.Value;

            component.Kitchen_Sink_Accessory_Grid_Features =
                arg.Properties.FirstOrDefault(x =>
                    x.Name.Equals(nameof(StyleSpecification.Kitchen_Sink_Accessory_Grid_Features), StringComparison.OrdinalIgnoreCase))?.Value;

            component.Sink_Accessory_Type =
               arg.Properties.FirstOrDefault(x =>
                   x.Name.Equals(nameof(StyleSpecification.Sink_Accessory_Type), StringComparison.OrdinalIgnoreCase))?.Value;

            component.Tub_Exterior_Shape =
               arg.Properties.FirstOrDefault(x =>
                   x.Name.Equals(nameof(StyleSpecification.Tub_Exterior_Shape), StringComparison.OrdinalIgnoreCase))?.Value;


            component.Tub_Features =
               arg.Properties.FirstOrDefault(x =>
                   x.Name.Equals(nameof(StyleSpecification.Tub_Features), StringComparison.OrdinalIgnoreCase))?.Value;

            component.Tub_Product_Type =
                arg.Properties.FirstOrDefault(x =>
                    x.Name.Equals(nameof(StyleSpecification.Tub_Product_Type), StringComparison.OrdinalIgnoreCase))?.Value;

            component.Bowl_Included_YN =
                arg.Properties.FirstOrDefault(x =>
                    x.Name.Equals(nameof(StyleSpecification.Bowl_Included_YN), StringComparison.OrdinalIgnoreCase))?.Value;

            component.Item_Shape =
               arg.Properties.FirstOrDefault(x =>
                   x.Name.Equals(nameof(StyleSpecification.Item_Shape), StringComparison.OrdinalIgnoreCase))?.Value;


            component.Toilet_Activation_Lever_Location =
                arg.Properties.FirstOrDefault(x =>
                    x.Name.Equals(nameof(StyleSpecification.Toilet_Activation_Lever_Location), StringComparison.OrdinalIgnoreCase))?.Value;

            component.Sink_Bath_Type =
                arg.Properties.FirstOrDefault(x =>
                x.Name.Equals(nameof(StyleSpecification.Sink_Bath_Type), StringComparison.OrdinalIgnoreCase))?.Value;



            viewProperty = arg.Properties.FirstOrDefault(x => x.Name.Equals(nameof(StyleSpecification.Sink_No_Of_Basins), StringComparison.OrdinalIgnoreCase));
            if (!string.IsNullOrEmpty(viewProperty?.Value) && int.TryParse(viewProperty.Value, out tempIntValue))
                component.Sink_No_Of_Basins = tempIntValue;


            component.Trim_Kit_Included_YN =
           arg.Properties.FirstOrDefault(x =>
               x.Name.Equals(nameof(StyleSpecification.Trim_Kit_Included_YN), StringComparison.OrdinalIgnoreCase))?.Value;


            component.Kitchen_Sink_Accessory_Rack_Type =
                arg.Properties.FirstOrDefault(x =>
                    x.Name.Equals(nameof(StyleSpecification.Kitchen_Sink_Accessory_Rack_Type), StringComparison.OrdinalIgnoreCase))?.Value;

            viewProperty = arg.Properties.FirstOrDefault(x => x.Name.Equals(nameof(StyleSpecification.Tub_Shower_Door_Fits_Opening_Height), StringComparison.OrdinalIgnoreCase));
            if (!string.IsNullOrEmpty(viewProperty?.Value) && decimal.TryParse(viewProperty.Value, out tempDoubleValue))
                component.Tub_Shower_Door_Fits_Opening_Height = tempDoubleValue;

            viewProperty = arg.Properties.FirstOrDefault(x => x.Name.Equals(nameof(StyleSpecification.Tub_Shower_Door_Glass_Thickness), StringComparison.OrdinalIgnoreCase));
            if (!string.IsNullOrEmpty(viewProperty?.Value) && int.TryParse(viewProperty.Value, out tempIntValue))
                component.Tub_Shower_Door_Glass_Thickness = tempIntValue;

            component.Door_Glass_Style =
                arg.Properties.FirstOrDefault(x =>
                    x.Name.Equals(nameof(StyleSpecification.Door_Glass_Style), StringComparison.OrdinalIgnoreCase))?.Value;

            component.Door_Type =
                arg.Properties.FirstOrDefault(x =>
                    x.Name.Equals(nameof(StyleSpecification.Door_Type), StringComparison.OrdinalIgnoreCase))?.Value;

            component.Frame_Type =
                arg.Properties.FirstOrDefault(x =>
                    x.Name.Equals(nameof(StyleSpecification.Frame_Type), StringComparison.OrdinalIgnoreCase))?.Value;


            viewProperty = arg.Properties.FirstOrDefault(x => x.Name.Equals(nameof(StyleSpecification.Number_Of_Flush_Valves), StringComparison.OrdinalIgnoreCase));
            if (!string.IsNullOrEmpty(viewProperty?.Value) && int.TryParse(viewProperty.Value, out tempIntValue))
                component.Number_Of_Flush_Valves = tempIntValue;

            component.Rough_In_Valve_Included_YN =
          arg.Properties.FirstOrDefault(x =>
              x.Name.Equals(nameof(StyleSpecification.Rough_In_Valve_Included_YN), StringComparison.OrdinalIgnoreCase))?.Value;

            component.Bidet_Slow_Close_Lid_YN =
         arg.Properties.FirstOrDefault(x =>
             x.Name.Equals(nameof(StyleSpecification.Bidet_Slow_Close_Lid_YN), StringComparison.OrdinalIgnoreCase))?.Value;

            component.Bath_Accessory_Hanger_Type =
                arg.Properties.FirstOrDefault(x =>
                    x.Name.Equals(nameof(StyleSpecification.Bath_Accessory_Hanger_Type), StringComparison.OrdinalIgnoreCase))?.Value;

            component.Bath_Accessory_Hardware_Included_YN =
        arg.Properties.FirstOrDefault(x =>
            x.Name.Equals(nameof(StyleSpecification.Bath_Accessory_Hardware_Included_YN), StringComparison.OrdinalIgnoreCase))?.Value;



            component.Bath_Accessory_Holder_Type =
                arg.Properties.FirstOrDefault(x =>
                    x.Name.Equals(nameof(StyleSpecification.Bath_Accessory_Holder_Type), StringComparison.OrdinalIgnoreCase))?.Value;

            component.Bath_Accessory_Hook_Type =
                arg.Properties.FirstOrDefault(x =>
                    x.Name.Equals(nameof(StyleSpecification.Bath_Accessory_Hook_Type), StringComparison.OrdinalIgnoreCase))?.Value;

            component.Bath_Accessory_Mount_Type =
                arg.Properties.FirstOrDefault(x =>
                    x.Name.Equals(nameof(StyleSpecification.Bath_Accessory_Mount_Type), StringComparison.OrdinalIgnoreCase))?.Value;

            component.Bath_Accessory_Mounting_Hardware_YN =
       arg.Properties.FirstOrDefault(x =>
           x.Name.Equals(nameof(StyleSpecification.Bath_Accessory_Mounting_Hardware_YN), StringComparison.OrdinalIgnoreCase))?.Value;

            component.Bath_Accessory_Type =
               arg.Properties.FirstOrDefault(x =>
                   x.Name.Equals(nameof(StyleSpecification.Bath_Accessory_Type), StringComparison.OrdinalIgnoreCase))?.Value;

            viewProperty = arg.Properties.FirstOrDefault(x => x.Name.Equals(nameof(StyleSpecification.Bath_Accessory_No_Of_Bars), StringComparison.OrdinalIgnoreCase));
            if (!string.IsNullOrEmpty(viewProperty?.Value) && int.TryParse(viewProperty.Value, out tempIntValue))
                component.Bath_Accessory_No_Of_Bars = tempIntValue;

            viewProperty = arg.Properties.FirstOrDefault(x => x.Name.Equals(nameof(StyleSpecification.Bath_Accessory_No_Of_Chambers), StringComparison.OrdinalIgnoreCase));
            if (!string.IsNullOrEmpty(viewProperty?.Value) && int.TryParse(viewProperty.Value, out tempIntValue))
                component.Bath_Accessory_No_Of_Chambers = tempIntValue;

            viewProperty = arg.Properties.FirstOrDefault(x => x.Name.Equals(nameof(StyleSpecification.Bath_Accessory_No_Of_Hangers), StringComparison.OrdinalIgnoreCase));
            if (!string.IsNullOrEmpty(viewProperty?.Value) && int.TryParse(viewProperty.Value, out tempIntValue))
                component.Bath_Accessory_No_Of_Hangers = tempIntValue;

            viewProperty = arg.Properties.FirstOrDefault(x => x.Name.Equals(nameof(StyleSpecification.Bath_Accessory_No_Of_Hooks), StringComparison.OrdinalIgnoreCase));
            if (!string.IsNullOrEmpty(viewProperty?.Value) && int.TryParse(viewProperty.Value, out tempIntValue))
                component.Bath_Accessory_No_Of_Hooks = tempIntValue;

            viewProperty = arg.Properties.FirstOrDefault(x => x.Name.Equals(nameof(StyleSpecification.Bath_Accessory_No_Of_Pieces), StringComparison.OrdinalIgnoreCase));
            if (!string.IsNullOrEmpty(viewProperty?.Value) && int.TryParse(viewProperty.Value, out tempIntValue))
                component.Bath_Accessory_No_Of_Pieces = tempIntValue;

            component.Bath_Accessory_Product_Type =
                arg.Properties.FirstOrDefault(x =>
                    x.Name.Equals(nameof(StyleSpecification.Bath_Accessory_Product_Type), StringComparison.OrdinalIgnoreCase))?.Value;

            component.Bath_Accessory_Textured_Grip_YN =
      arg.Properties.FirstOrDefault(x =>
          x.Name.Equals(nameof(StyleSpecification.Bath_Accessory_Textured_Grip_YN), StringComparison.OrdinalIgnoreCase))?.Value;


            component.Shower_Corrosion_Resistant_YN =
   arg.Properties.FirstOrDefault(x =>
       x.Name.Equals(nameof(StyleSpecification.Shower_Corrosion_Resistant_YN), StringComparison.OrdinalIgnoreCase))?.Value;

            component.Temperature_Control_YN =
           arg.Properties.FirstOrDefault(x =>
               x.Name.Equals(nameof(StyleSpecification.Temperature_Control_YN), StringComparison.OrdinalIgnoreCase))?.Value;


            component.Foot_Operation_YN =
           arg.Properties.FirstOrDefault(x =>
               x.Name.Equals(nameof(StyleSpecification.Foot_Operation_YN), StringComparison.OrdinalIgnoreCase))?.Value;

            component.Fitting_Touchless_Touch_On =
           arg.Properties.FirstOrDefault(x =>
               x.Name.Equals(nameof(StyleSpecification.Fitting_Touchless_Touch_On), StringComparison.OrdinalIgnoreCase))?.Value;


            component.Water_Efficient_YN =
           arg.Properties.FirstOrDefault(x =>
               x.Name.Equals(nameof(StyleSpecification.Water_Efficient_YN), StringComparison.OrdinalIgnoreCase))?.Value;


            component.Drininking_Chilled_Water_YN =
               arg.Properties.FirstOrDefault(x =>
                   x.Name.Equals(nameof(StyleSpecification.Drininking_Chilled_Water_YN), StringComparison.OrdinalIgnoreCase))?.Value;

            component.Drinking_Hot_Water_YN =
              arg.Properties.FirstOrDefault(x =>
                  x.Name.Equals(nameof(StyleSpecification.Drinking_Hot_Water_YN), StringComparison.OrdinalIgnoreCase))?.Value;

            component.Drinking_Water_Dispenser_YN =
                arg.Properties.FirstOrDefault(x =>
                    x.Name.Equals(nameof(StyleSpecification.Drinking_Water_Dispenser_YN), StringComparison.OrdinalIgnoreCase))?.Value;


            component.Shower_Wall_Type =
             arg.Properties.FirstOrDefault(x =>
                 x.Name.Equals(nameof(StyleSpecification.Shower_Wall_Type), StringComparison.OrdinalIgnoreCase))?.Value;

            component.Shower_Bath_Product_Type =
             arg.Properties.FirstOrDefault(x =>
                 x.Name.Equals(nameof(StyleSpecification.Shower_Bath_Product_Type), StringComparison.OrdinalIgnoreCase))?.Value;


            component.Valve_Type =
            arg.Properties.FirstOrDefault(x =>
                x.Name.Equals(nameof(StyleSpecification.Valve_Type), StringComparison.OrdinalIgnoreCase))?.Value;

            component.Shower_System_Product_Type =
            arg.Properties.FirstOrDefault(x =>
                x.Name.Equals(nameof(StyleSpecification.Shower_System_Product_Type), StringComparison.OrdinalIgnoreCase))?.Value;



            context.Logger.LogInformation("Current Entity Version : " + entity.Version);

            // Persist changes
            this.Commander.Pipeline<IPersistEntityPipeline>().Run(new PersistEntityArgument(entity), context);

            return Task.FromResult(arg);
        }
    }
}
