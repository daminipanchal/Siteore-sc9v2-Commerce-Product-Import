using Sitecore.Commerce.Core;
using System.ComponentModel.DataAnnotations;

namespace LX.Plugin.SellableItem.StiboAttributes.Components
{
    public class ProductFeatures : Component
    {
        [Display(Name = "About This Product")]
        public string Marketing_Claims_1 { get; set; } = string.Empty;

        [Display(Name = "Mirror Type")]
        public string Mirror_Bath_Product_Type { get; set; } = string.Empty;

        [Display(Name = "Beveled Mirror Frame")]
        public string Mirror_Beveled_Frame_YN { get; set; } = string.Empty;

        [Display(Name = "Finish Type - Mirror Frame")]
        public string Mirror_Frame_Finish_Family { get; set; } = string.Empty;

        [Display(Name = "Fitting Features")]
        public string Fitting_Features { get; set; } = string.Empty;

        [Display(Name = "Feature Bullets 1")]
        public string Feature_Bullets_1 { get; set; } = string.Empty;

        [Display(Name = "Feature Bullets 2")]
        public string Feature_Bullets_2 { get; set; } = string.Empty;

        [Display(Name = "Feature Bullets 3")]
        public string Feature_Bullets_3 { get; set; } = string.Empty;

        [Display(Name = "Feature Bullets 4")]
        public string Feature_Bullets_4 { get; set; } = string.Empty;

        [Display(Name = "Feature Bullets 5")]
        public string Feature_Bullets_5 { get; set; } = string.Empty;

        [Display(Name = "Feature Bullets 6")]
        public string Feature_Bullets_6 { get; set; } = string.Empty;

        [Display(Name = "Feature Bullets 7")]
        public string Feature_Bullets_7 { get; set; } = string.Empty;

        [Display(Name = "Feature Bullets 8")]
        public string Feature_Bullets_8 { get; set; } = string.Empty;

        [Display(Name = "Feature Bullets 9")]
        public string Feature_Bullets_9 { get; set; } = string.Empty;

        [Display(Name = "Feature Bullets 10")]
        public string Feature_Bullets_10 { get; set; } = string.Empty;

    }
}
