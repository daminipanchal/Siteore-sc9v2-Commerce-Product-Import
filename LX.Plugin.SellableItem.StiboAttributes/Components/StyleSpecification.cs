using Sitecore.Commerce.Core;
using System.ComponentModel.DataAnnotations;

namespace LX.Plugin.SellableItem.StiboAttributes.Components
{
    public class StyleSpecification : Component
    {
        [Display(Name = "Integral Dish Rack")]
        public string Sink_Integral_Dish_Rack_YN { get; set; } = string.Empty;

        [Display(Name = "Drain Included")]
        public string Drain_Included_YN { get; set; } = string.Empty;

        [Display(Name = "Finish Type")]
        public string Vanity_Hardware_Finish_Family { get; set; } = string.Empty;

        [Display(Name = "Drain Material")]
        public string Drain_Material { get; set; } = string.Empty;

        [Display(Name = "Base Threshold")]
        public int Base_Threshold { get; set; } = 0;

        [Display(Name = "Finish Type - Door Glass")]
        public string Door_Glass_Finish { get; set; } = string.Empty;

        [Display(Name = "Bristle Material")]
        public string Bristle_Material { get; set; } = string.Empty;

        [Display(Name = "Deck Plate Included")]
        public string Deck_Plate_Included_YN { get; set; } = string.Empty;

        [Display(Name = "Shower Trim Type")]
        public string Shower_Trim_Type { get; set; } = string.Empty;

        [Display(Name = "Shower Valve Type")]
        public string Shower_Valve_Type { get; set; } = string.Empty;

        [Display(Name = "Installation Type - Wall")]
        public string Shower_Wall_Installation_Type { get; set; } = string.Empty;

        [Display(Name = "Showerhead Included")]
        public string Showerhead_Included_YN { get; set; } = string.Empty;

        [Display(Name = "Side Spray")]
        public string Side_Spray_YN { get; set; } = string.Empty;

        [Display(Name = "Finish Type - Sink")]
        public string Sink_Finish_Type { get; set; } = string.Empty;

        [Display(Name = "Item Shape - Sink")]
        public string Sink_Shape { get; set; } = string.Empty;

        [Display(Name = "Soap Lotion Dispenser")]
        public string Soap_Lotion_Dispenser_YN { get; set; } = string.Empty;

        [Display(Name = "Trim Kit Included")]
        public string Sink_Rack_Type { get; set; } = string.Empty;

        [Display(Name = "Type")]
        public string Sink_Product_Type { get; set; } = string.Empty;

        [Display(Name = "Water Filter Included")]
        public string Water_Filter_Included_YN { get; set; } = string.Empty;

        [Display(Name = "Spray Type")]
        public string Spray_Pattern_List { get; set; } = string.Empty;

        [Display(Name = "Mirror Frame Material")]
        public string Mirror_Frame_Material { get; set; } = string.Empty;

        [Display(Name = "Framed Mirrors/Frameless Mirror")]
        public string Mirror_Framed_Or_Frameless { get; set; } = string.Empty;

        [Display(Name = "Mirror Light")]
        public string Mirror_Light_YN { get; set; } = string.Empty;

        [Display(Name = "Magnifying Mirror")]
        public string Mirror_Magnifying_YN { get; set; } = string.Empty;

        [Display(Name = "Item Shape - Mirror")]
        public string Mirror_Shape { get; set; } = string.Empty;

        [Display(Name = "Telescoping Mirror")]
        public string Mirror_Telescoping_YN { get; set; } = string.Empty;

        [Display(Name = "Mirror Type")]
        public string Mirror_Type { get; set; } = string.Empty;

        [Display(Name = "Product Style")]
        public string Product_Style { get; set; } = string.Empty;

        [Display(Name = "Pull Down")]
        public string Pull_Down_YN { get; set; } = string.Empty;

        [Display(Name = "Shower Arm Style")]
        public string Shower_Arm_Style { get; set; } = string.Empty;

        [Display(Name = "Shower Diverter Type")]
        public string Shower_Diverter_Type { get; set; } = string.Empty;

        [Display(Name = "Shower No. of Spray Settings")]
        public int Shower_No_Of_Spray_Settings { get; set; } = 0;

        [Display(Name = "Shower No. of Jets")]
        public int Shower_No_Of_Jets { get; set; } = 0;

        [Display(Name = "Fitting Material")]
        public string Fitting_Material { get; set; } = string.Empty;

        [Display(Name = "Fitting Mount Type")]
        public string Fitting_Mount_Type { get; set; } = string.Empty;

        [Display(Name = "No. of Handles")]
        public int Fitting_No_Of_Handles { get; set; } = 0;

        [Display(Name = "Spout Height")]
        public string Fitting_Spout_Type { get; set; } = string.Empty;

        [Display(Name = "Hinge Material")]
        public string Hinge_Material { get; set; } = string.Empty;

        [Display(Name = "Fitting Material")]
        public string Fixture_Material { get; set; } = string.Empty;

        [Display(Name = "Finish Type - Frame")]
        public string Frame_Finish { get; set; } = string.Empty;

        [Display(Name = "Item Shape - Hand Head Shower")]
        public string Hand_Head_Shower_Shape { get; set; } = string.Empty;

        [Display(Name = "Finish Type - Handle")]
        public string Handle_Finish { get; set; } = string.Empty;

        [Display(Name = "Maximum Occupants")]
        public int Max_Occupants { get; set; } = 0;

        [Display(Name = "Flushing Type")]
        public string Flushing_Type { get; set; } = string.Empty;

        [Display(Name = "Door Swing")]
        public string Door_Swing { get; set; } = string.Empty;

        [Display(Name = "Shower Wall No. of Shelves")]
        public int Shower_Wall_Number_Of_Shelves { get; set; } = 0;

        [Display(Name = "Handle Style")]
        public string Handle_Style { get; set; } = string.Empty;

        [Display(Name = "Jet Type")]
        public string Jet_Type { get; set; } = string.Empty;

        [Display(Name = "Tub Lighting")]
        public string Tub_Lighting { get; set; } = string.Empty;

        [Display(Name = "Features - Kitchen Sink Accessory")]
        public string Kitchen_Sink_Accessory_Grid_Features { get; set; } = string.Empty;

        [Display(Name = "Accessory Type")]
        public string Sink_Accessory_Type { get; set; } = string.Empty;

        [Display(Name = "Style - Tub Exterior")]
        public string Tub_Exterior_Shape { get; set; } = string.Empty;

        [Display(Name = "Features")]
        public string Tub_Features { get; set; } = string.Empty;

        [Display(Name = "Tub Type")]
        public string Tub_Product_Type { get; set; } = string.Empty;


        [Display(Name = "Bowl Included")]
        public string Bowl_Included_YN { get; set; } = string.Empty;

        [Display(Name = "Item Shape - Base")]
        public string Item_Shape { get; set; } = string.Empty;

        [Display(Name = "Activation Lever Location")]
        public string Toilet_Activation_Lever_Location { get; set; } = string.Empty;

        [Display(Name = "Sink Bath Type")]
        public string Sink_Bath_Type { get; set; } = string.Empty;

        [Display(Name = "No. of Sink Basins")]
        public int Sink_No_Of_Basins { get; set; } = 0;

        [Display(Name = "Trim Kit Included")]
        public string Trim_Kit_Included_YN { get; set; } = string.Empty;

        [Display(Name = "Kitchen Sink Accessory Rack type")]
        public string Kitchen_Sink_Accessory_Rack_Type { get; set; } = string.Empty;

        [Display(Name = "Tub Shower Door Fits Opening Height")]
        public decimal Tub_Shower_Door_Fits_Opening_Height { get; set; } = 0;

        [Display(Name = "Tub Shower Door Glass Thickness")]
        public int Tub_Shower_Door_Glass_Thickness { get; set; } = 0;

        [Display(Name = "Door Glass Style")]
        public string Door_Glass_Style { get; set; } = string.Empty;

        [Display(Name = "Door Type")]
        public string Door_Type { get; set; } = string.Empty;

        [Display(Name = "Frame Type")]
        public string Frame_Type { get; set; } = string.Empty;

        [Display(Name = "No. of Flush Valves")]
        public int Number_Of_Flush_Valves { get; set; } = 0;

        [Display(Name = "Rough In Valve Included")]
        public string Rough_In_Valve_Included_YN { get; set; } = string.Empty;

        [Display(Name = "Slow Close Seat")]
        public string Bidet_Slow_Close_Lid_YN { get; set; } = string.Empty;

        [Display(Name = "Bath Accessory Hanger Type")]
        public string Bath_Accessory_Hanger_Type { get; set; } = string.Empty;

        [Display(Name = "Accessory Hardware Included")]
        public string Bath_Accessory_Hardware_Included_YN { get; set; } = string.Empty;

        [Display(Name = "Accessory Holder Type")]
        public string Bath_Accessory_Holder_Type { get; set; } = string.Empty;

        [Display(Name = "Accessory Hook Type")]
        public string Bath_Accessory_Hook_Type { get; set; } = string.Empty;

        [Display(Name = "Accessory Mount Type")]
        public string Bath_Accessory_Mount_Type { get; set; } = string.Empty;

        [Display(Name = "Bath Accessory Mounting Hardware")]
        public string Bath_Accessory_Mounting_Hardware_YN { get; set; } = string.Empty;

        [Display(Name = "No. of Bars")]
        public int Bath_Accessory_No_Of_Bars { get; set; } = 0;

        [Display(Name = "No. of Chambers")]
        public int Bath_Accessory_No_Of_Chambers { get; set; } = 0;

        [Display(Name = "No. of Hangers")]
        public int Bath_Accessory_No_Of_Hangers { get; set; } = 0;

        [Display(Name = "No. of Hooks")]
        public int Bath_Accessory_No_Of_Hooks { get; set; } = 0;

        [Display(Name = "No. of Pieces")]
        public int Bath_Accessory_No_Of_Pieces { get; set; } = 0;

        [Display(Name = "Product Type")]
        public string Bath_Accessory_Product_Type { get; set; } = string.Empty;

        [Display(Name = "Textured Grip")]
        public string Bath_Accessory_Textured_Grip_YN { get; set; } = string.Empty;

        [Display(Name = "Bathroom Accessory Type")]
        public string Bath_Accessory_Type { get; set; } = string.Empty;

        [Display(Name = "Shower Corrosion Resistant")]
        public string Shower_Corrosion_Resistant_YN { get; set; } = string.Empty;

        [Display(Name = "Temperature Control")]
        public string Temperature_Control_YN { get; set; } = string.Empty;

        [Display(Name = "Faucet Type")]
        public string Fitting_Touchless_Touch_On { get; set; } = string.Empty;

        [Display(Name = "Foot Operation")]
        public string Foot_Operation_YN { get; set; } = string.Empty;

        [Display(Name = "Water Efficient")]
        public string Water_Efficient_YN { get; set; } = string.Empty;

        [Display(Name = "Drininking Chilled Water")]
        public string Drininking_Chilled_Water_YN { get; set; } = string.Empty;

        [Display(Name = "Drinking Hot Water")]
        public string Drinking_Hot_Water_YN { get; set; } = string.Empty;

        [Display(Name = "Drinking Water Dispenser")]
        public string Drinking_Water_Dispenser_YN { get; set; } = string.Empty;

        [Display(Name = "Shower Wall Type")]
        public string Shower_Wall_Type { get; set; } = string.Empty;

        [Display(Name = "Shower Bath Product Type")]
        public string Shower_Bath_Product_Type { get; set; } = string.Empty;

        [Display(Name = "Valve Type")]
        public string Valve_Type { get; set; } = string.Empty;

        [Display(Name = "Shower System Product Type")]
        public string Shower_System_Product_Type { get; set; } = string.Empty;
        [Display(Name = "Sink Type")]
        public string Kitchen_Sink_Product_Type { get; set; } = string.Empty;
    }
}
