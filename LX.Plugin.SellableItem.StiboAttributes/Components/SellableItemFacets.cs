using Sitecore.Commerce.Core;
using System.ComponentModel.DataAnnotations;

namespace LX.Plugin.SellableItem.StiboAttributes.Components
{
    public class SellableItemFacets : Component
    {
        [Display(Name = "Item Shape")]
        public string Item_Shape { get; set; } = string.Empty;

        [Display(Name = "Features")]
        public string Features { get; set; } = string.Empty;

        [Display(Name = "Certifications")]
        public string Certifications { get; set; } = string.Empty;

        [Display(Name = "Water Type")]
        public string Water_Type { get; set; } = string.Empty;

        [Display(Name = "Shower Type")]
        public string Shower_Type { get; set; } = string.Empty;

        [Display(Name = "Dimensions")]
        public string Dimensions { get; set; } = string.Empty;

        [Display(Name = "Flow Rate")]
        public string Flow_Rate { get; set; } = string.Empty;

        [Display(Name = "Tub Dimensions")]
        public string Tub_Dimensions { get; set; } = string.Empty;

        [Display(Name = "Vanity Assembled Dimensions")]
        public string Vanity_Assembled_Dimensions { get; set; } = string.Empty;

        [Display(Name = "Vanity Cabinet Dimensions")]
        public string Vanity_Cabinet_Dimensions { get; set; } = string.Empty;

        [Display(Name = "Drain Opening Size")]
        public string Drain_Opening_Size { get; set; } = string.Empty;

        [Display(Name = "Toilet Seat Inside Length")]
        public string Toilet_Seat_Inside_Length { get; set; } = string.Empty;
    }
}
