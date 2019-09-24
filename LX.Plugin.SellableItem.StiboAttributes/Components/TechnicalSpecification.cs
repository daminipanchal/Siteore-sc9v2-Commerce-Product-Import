using Sitecore.Commerce.Core;
using System.ComponentModel.DataAnnotations;

namespace LX.Plugin.SellableItem.StiboAttributes.Components
{
    public class TechnicalSpecification : Component
    {
        [Display(Name = "Mirror Hardware Included")]
        public string Mirror_Hardware_Included_YN { get; set; } = string.Empty;

        [Display(Name = "Mirror Mount Location")]
        public string Mirror_Mount_Location { get; set; } = string.Empty;

        [Display(Name = "No. Output Valves Connection")]
        public int Number_Output_Valves_Connection { get; set; } = 0;


        [Display(Name = "Output Connection Type")]
        public string Output_Connection_Type { get; set; } = string.Empty;

        [Display(Name = "Shower Rough in Valve")]
        public int Shower_Rough_In_Valve { get; set; } = 0;

        [Display(Name = "Fitting Connection Size")]
        public string Fitting_Connection_Size_Input { get; set; }

        [Display(Name = "Fitting Handle Height")]
        public string Fitting_Handle_Height { get; set; }

        [Display(Name = "Fitting Handle Type")]
        public string Fitting_Handle_Type { get; set; } = string.Empty;

        [Display(Name = "Fitting Hose Length")]
        public decimal Fitting_Hose_Length { get; set; } = 0;


        [Display(Name = "Fitting Max Deck Thickness")]
        public string Fitting_Max_Deck_Thickness { get; set; }

        [Display(Name = "Fitting Max Deck Thickness with Escutcheon")]
        public int Fitting_Max_Deck_Thickness_w_Escutcheon { get; set; } = 0;


        [Display(Name = "Fitting No. of Holes Required")]
        public int Fitting_No_Of_Holes_Required { get; set; } = 0;

        [Display(Name = "Fitting Speed Connect")]
        public string Fitting_Speed_Connect_YN { get; set; } = string.Empty;

        [Display(Name = "Dimensions")]
        public string Fitting_Spout_Height { get; set; } 

        [Display(Name = "Fitting Spout Reach")]
        public string Fitting_Spout_Reach { get; set; }

        [Display(Name = "Tub Depth")]
        public string Tub_Depth { get; set; } = string.Empty;

        [Display(Name = "Tub Overflow Height")]
        public string Tub_Overflow_Height { get; set; } = string.Empty;

        [Display(Name = "Tub Water Depth")]
        public string Tub_Water_Depth { get; set; } = string.Empty;


        [Display(Name = "Fitting swivel Degrees")]
        public int Fitting_Swivel_Degrees { get; set; } = 0;


        [Display(Name = "Fitting Valve Needed for Install")]
        public string Fitting_Valve_Needed_For_Install_YN { get; set; } = string.Empty;


        [Display(Name = "Flow Rate")]
        public string FlowRateGPC { get; set; } = string.Empty;

        [Display(Name = "Handle Length")]
        public decimal Handle_Length { get; set; } = 0;

        [Display(Name = "Operating Pressure Static")]
        public int Operating_Pressure_Static { get; set; } = 0;

        [Display(Name = "Handle To Handle Measurement")]
        public string Handle_To_Handle_Measurement { get; set; } = string.Empty;


        [Display(Name = "Fitting Valve Material")]
        public string Fitting_Valve_Material { get; set; } = string.Empty;

        [Display(Name = "Hose Included")]
        public string Hose_Included_YN { get; set; } = string.Empty;


        [Display(Name = "Input Connection Type")]
        public string Input_Connection_Type { get; set; } = string.Empty;

        [Display(Name = "Installation Hardware Included")]
        public string Installation_Hardware_Included { get; set; } = string.Empty;

        [Display(Name = "Tub Wattage")]
        public string Tub_Wattage { get; set; } = string.Empty;

        [Display(Name = "Tub Blower Placement")]
        public string Tub_Blower_Placement { get; set; } = string.Empty;

        [Display(Name = "Tub Pump Placement")]
        public string Tub_Pump_Placement { get; set; } = string.Empty;

        [Display(Name = "Tub Pump Speeds")]
        public int Tub_Pump_Speeds { get; set; } = 0;

        [Display(Name = "Tub Sound Dampening")]
        public string Tub_Sound_Dampening { get; set; } = string.Empty;

        [Display(Name = "Tank Included")]
        public string Tank_Included_YN { get; set; } = string.Empty;

        [Display(Name = "Sink Predrilled Holes")]
        public string Sink_Predrilled_Holes_YN { get; set; } = string.Empty;

        [Display(Name = "Number of Trapways")]
        public string Number_Of_Toilet_Trapways { get; set; } = string.Empty;

        [Display(Name = "Mold")]
        public string Mold { get; set; } = string.Empty;

        [Display(Name = "Number of Toilet Flush Valves")]
        public int Number_Of_Toilet_Flush_Valves { get; set; } = 0;

        [Display(Name = "No. of Dryer Settings")]
        public int Bidet_No_Of_Dryer_Settings { get; set; } = 0;

        [Display(Name = "No. of Spray Nozzles")]
        public int Bidet_No_Of_Spray_Nozzles { get; set; } = 0;

        [Display(Name = "No. of Water Control Settings")]
        public int Bidet_No_Of_Water_Control_Settings { get; set; } = 0;

        [Display(Name = "Bidet Spray Type")]
        public string Bath_Fitting_Bidet_Spray_Type { get; set; } = string.Empty;

        [Display(Name = "Power Cord Length")]
        public int Power_Cord_Length { get; set; } = 0;

        [Display(Name = "Toilet Seat Front Type")]
        public string Toilet_Seat_Front_Type { get; set; } = string.Empty;

        [Display(Name = "Flushing Mechanism")]
        public string Flushing_Mechanism { get; set; } = string.Empty;

        [Display(Name = "Bidet Remote Included")]
        public string Bidet_Remote_Included_YN { get; set; } = string.Empty;

        [Display(Name = "Vanity Assembled Depth")]
        public string Vanity_Assembled_Depth { get; set; } = string.Empty;

        [Display(Name = "Vanity Cabinet Depth")]
        public string Vanity_Cabinet_Depth { get; set; } = string.Empty;

        [Display(Name = "Vanity Cabinet Height")]
        public string Vanity_Cabinet_Height { get; set; } = string.Empty;

        [Display(Name = "Vanity Cabinet Width")]
        public string Vanity_Cabinet_Width { get; set; } = string.Empty;

        [Display(Name = "Vanity Assembled Width")]
        public string Vanity_Assembled_Width { get; set; } = string.Empty;

        [Display(Name = "No of Batteries Included")]
        public string No_Of_Batteries_Included { get; set; } = string.Empty;

        [Display(Name = "Tub Drain Opening Size")]
        public string Tub_Drain_Opening_Size { get; set; } = string.Empty;

        [Display(Name = "Tub Shower Base Threshold")]
        public string Tub_Shower_Base_Threshold_Height { get; set; } = string.Empty;

        [Display(Name = "No. of Finished Sides")]
        public int Shower_Base_No_Finished_Sides { get; set; } = 0;

        [Display(Name = "No. of Curbs")]
        public int Shower_Base_Number_Of_Curbs { get; set; } = 0;

        [Display(Name = "Door Glass Thickness")]
        public string Door_Glass_Thickness { get; set; } = string.Empty;

        [Display(Name = "Door Hinged")]
        public string Door_Hinged_YN { get; set; } = string.Empty;

        [Display(Name = "Toilet Seat Width Inside")]
        public string Toilet_Seat_Width_Inside { get; set; } = string.Empty;

        [Display(Name = "Toilet Seat Length Inside")]
        public string Toilet_Seat_Length_Inside { get; set; } = string.Empty;

        [Display(Name = "Door Usage")]
        public string Door_Usage { get; set; } = string.Empty;

        [Display(Name = "Drain Location")]
        public string Drain_Location { get; set; } = string.Empty;

        [Display(Name = "Towel Bar Included")]
        public string Towel_Bar_Included_YN { get; set; } = string.Empty;

        [Display(Name = "Door Width")]
        public decimal Tub_Shower_Door_Width { get; set; } = 0;

        [Display(Name = "Caulkless Shower Wall")]
        public string Shower_Wall_Caulkless_YN { get; set; } = string.Empty;

        [Display(Name = "Shower Wall Construction")]
        public string Shower_Wall_Construction { get; set; } = string.Empty;

        [Display(Name = "Shower Wall Surface")]
        public string Shower_Wall_Surface { get; set; } = string.Empty;

        [Display(Name = "Bowl Right Side Below Counter Depth")]
        public decimal Bowl_Right_Side_Below_Counter_Depth { get; set; } = 0;

        [Display(Name = "Sink Cut-Out Below Counter Depth")]
        public decimal Sink_Cut_Out_Below_Counter_Depth { get; set; } = 0;

        [Display(Name = "Sink Cut-Out Front to Back Width")]
        public decimal Sink_Cut_Out_Front_To_Back_Width { get; set; } = 0;

        [Display(Name = "Sink Cut-Out Left to Right Length")]
        public decimal Sink_Cut_Out_Left_To_Right_Length { get; set; } = 0;

        [Display(Name = "Depth to Overflow")]
        public decimal Sink_Depth_To_Overflow { get; set; } = 0;

        [Display(Name = "Minimum Cabinet Size")]
        public int Sink_Minimum_Cabinet_Size { get; set; } = 0;

        [Display(Name = "Sink Overflow Location")]
        public string Sink_Overflow_Location { get; set; } = string.Empty;

        [Display(Name = "Soaking Depth")]
        public decimal Soaking_Depth { get; set; } = 0;

        [Display(Name = "Tub Blower")]
        public string Tub_Blower { get; set; } = string.Empty;

        [Display(Name = "Tub Pump Amperage")]
        public int Tub_Pump_Amperage { get; set; } = 0;

        [Display(Name = "Tub Water Capacity")]
        public int Tub_Water_Capacity { get; set; } = 0;

        [Display(Name = "Sink Below Counter Depth")]
        public decimal Sink_Bowl_Below_Counter_Depth { get; set; } = 0;

        [Display(Name = "Sink Front to back Width")]
        public decimal Sink_Front_To_Back_Width { get; set; } = 0;

        [Display(Name = "Sink Left to Right Length")]
        public decimal Sink_Left_To_Right_Length { get; set; } = 0;

        [Display(Name = "Sink Top to Bottom Depth")]
        public decimal Sink_Top_To_Bottom_Depth { get; set; } = 0;

        [Display(Name = "Sump Length")]
        public decimal Sump_Length { get; set; } = 0;

        [Display(Name = "Sump Width")]
        public decimal Sump_Width { get; set; } = 0;

        [Display(Name = "Flow Rate")]
        public string Flow_Rate { get; set; } = string.Empty;

        [Display(Name = "Flush Technology")]
        public string Flush_Technology { get; set; } = string.Empty;

        [Display(Name = "Sink Bowl Split")]
        public string Sink_Bowl_Split { get; set; } = string.Empty;

        [Display(Name = "Map Performance Rating")]
        public int MAP_Performance_Rating { get; set; } = 0;

        [Display(Name = "Bowl Height Without Seat")]
        public decimal Bowl_Height_Without_Seat { get; set; } = 0;

        [Display(Name = "Tub Shower Door Height")]
        public decimal Tub_Shower_Door_Height { get; set; } = 0;

        [Display(Name = "Tub Shower Door Opening Width Max")]
        public int Tub_Shower_Door_Opening_Width_Max { get; set; } = 0;

        [Display(Name = "Tub Shower Door Opening Width Min")]
        public decimal Tub_Shower_Door_Opening_Width_Min { get; set; } = 0;

        [Display(Name = "Flush Valve Size")]
        public decimal Flush_Valve_Size { get; set; } = 0;

        [Display(Name = "Operating Pressure Flushing")]
        public int Operating_Pressure_Flushing { get; set; } = 0;

        [Display(Name = "Rough In Size")]
        public decimal Rough_In_Size { get; set; } = 0;

        [Display(Name = "Toilet Bowl Height with Seat")]
        public int Toilet_Bowl_Height_With_Seat { get; set; } = 0;

        [Display(Name = "Toilet Trapway Diameter")]
        public int Toilet_Trapway_Diameter { get; set; } = 0;

        [Display(Name = "No. of Heat Settings")]
        public int Bidet_No_Of_Heat_Settings { get; set; } = 0;

        [Display(Name = "Battery Weight")]
        public string Battery_Weight { get; set; } = string.Empty;

        [Display(Name = "Chemical Composition Of Battery")]
        public string Chemical_Composition_Of_Battery { get; set; } = string.Empty;

        [Display(Name = "Toilet Seat Bolt Spread")]
        public int Toilet_Seat_Bolt_Spread { get; set; } = 0;

        [Display(Name = "Escutcheon Internal Diameter")]
        public int Escutcheon_Internal_Diameter { get; set; } = 0;

        [Display(Name = "No. Input Valves connection")]
        public int Number_Input_Valves_Connection { get; set; } = 0;

        [Display(Name = "Bath Accessory Weight Capacity")]
        public int Bath_Accessory_Weight_Capacity { get; set; } = 0;

        [Display(Name = "Bristle Stiffness")]
        public int Bristle_Stiffness { get; set; } = 0;

        [Display(Name = "Bristle Length")]
        public int Bristle_Length { get; set; } = 0;

        [Display(Name = "Dimensions")]
        public string Shower_Arm_Reach { get; set; } = string.Empty;

        [Display(Name = "Dimensions")]
        public int Shower_Wall_Bar_Length { get; set; } = 0;

        [Display(Name = "Rough in valves Features")]
        public string Rough_In_Valve_Features { get; set; } = string.Empty;

        [Display(Name = "Installation")]
        public string Concealed_Or_Exposed_Flush_Valve { get; set; } = string.Empty;

        [Display(Name = "Assembly Required")]
        public string Assembly_Required_YN { get; set; } = string.Empty;

        [Display(Name = "Installation Type - Bath Accessory")]
        public string Bath_Accessory_Installation_Type { get; set; } = string.Empty;

        [Display(Name = "Bath Shower Diameter")]
        public int Bath_Shower_Diameter { get; set; } = 0;

        [Display(Name = "Bath Shower Maximum Deck Thickness")]
        public decimal Bath_Shower_Maximum_Deck_Thickness { get; set; } = 0;

        [Display(Name = "Bath Shower Showerhead Face Diameter")]
        public int Bath_Shower_Showerhead_Face_Diameter { get; set; } = 0;

        [Display(Name = "Bath Shower Sprayer Face Diameter")]
        public int Bath_Shower_Sprayer_Face_Diameter { get; set; } = 0;

        [Display(Name = "Batteries Included")]
        public string Batteries_Included_YN { get; set; } = string.Empty;

        [Display(Name = "CEC Compliant Product")]
        public string Compliant_Product_Substitute { get; set; } = string.Empty;

        [Display(Name = "EA Each Weight")]
        public decimal EA_Each_Weight { get; set; } = 0;

        [Display(Name = "Sink Faucet Drilling")]
        public int Faucet_Hole_Spacing { get; set; } = 0;

        [Display(Name = "Installation Type - Wall Bar")]
        public string Shower_Wall_Bar_Installation { get; set; } = string.Empty;

        [Display(Name = "Item Shape - Tub Interior")]
        public string Tub_Interior_Shape { get; set; } = string.Empty;

        [Display(Name = "Voltage")]
        public int Tub_Voltage { get; set; } = 0;

        [Display(Name = "Valve Application")]
        public string Valve_Application_YN { get; set; } = string.Empty;

        [Display(Name = "Where is your valve?")]
        public string Valve_Indoor_Outdoor { get; set; } = string.Empty;

        [Display(Name = "Valve Input Diameter")]
        public int Valve_Input_Diameter { get; set; } = 0;

        [Display(Name = "Valve Output Diameter")]
        public int Valve_Output_Diameter { get; set; } = 0;

        [Display(Name = "Valve Maximum Pressure")]
        public int Valves_Maximum_Pressure { get; set; } = 0;

        [Display(Name = "Valve Maximum Working Temperature")]
        public int Valves_Maximum_Working_Temperature { get; set; } = 0;

        [Display(Name = "Volume Control")]
        public string Volume_Control_YN { get; set; } = string.Empty;

        [Display(Name = "Item Volume")]
        public decimal Volume_SAP { get; set; } = 0;

        [Display(Name = "Maximum Weight Recommendation")]
        public int Weight_Capacity { get; set; } = 0;

        [Display(Name = "ADA")]
        public string ADA_YN { get; set; } = string.Empty;

        [Display(Name = "Barrier Free")]
        public string Barrier_Free { get; set; } = string.Empty;

        [Display(Name = "Massachusetts Plumbing Board")]
        public string Massachusetts_Plumbing_Board_YN { get; set; } = string.Empty;

        [Display(Name = "UL Listed")]
        public string Certification_CAN_UL { get; set; } = string.Empty;

        [Display(Name = "Smoke and Flame Compliant")]
        public string Smoke_And_Flame_Compliant { get; set; } = string.Empty;

        [Display(Name = "Proposition 65 Disclosure")]
        public string Proposition_65_Disclosure_YN { get; set; } = string.Empty;

        [Display(Name = "WaterSense")]
        public string EPA_Watersense_YN { get; set; } = string.Empty;

        [Display(Name = "Certifications")]
        public string Certification_UL { get; set; } = string.Empty;

        [Display(Name = "CEC Certified")]
        public string CEC_Certified_YN { get; set; } = string.Empty;

        [Display(Name = "Certification NSF")]
        public string Certification_NSF { get; set; } = string.Empty;

        [Display(Name = "CALGreen")]
        public string Cal_Green_YN { get; set; } = string.Empty;

        [Display(Name = "Vanity Assembled Height")]
        public string Vanity_Assembled_Height { get; set; } = string.Empty;

        [Display(Name = "Vanity Assembled Weight")]
        public string Vanity_Assembled_Weight { get; set; } = string.Empty;

        [Display(Name = "Height")]
        public string Product_Height { get; set; } = string.Empty;

        [Display(Name = "Weight")]
        public string Product_Weight { get; set; } = string.Empty;

        [Display(Name = "Length")]
        public string Product_Length { get; set; } = string.Empty;

        [Display(Name = "Width")]
        public string Product_Width { get; set; } = string.Empty;

    }
}
