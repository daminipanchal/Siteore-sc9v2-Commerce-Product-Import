using Sitecore.Commerce.Core;
using System.ComponentModel.DataAnnotations;

namespace LX.Plugin.SellableItem.StiboAttributes.Components
{
    public class ProductOverview : Component
    {
        [Display(Name = "UPC/EAN")]
        public string UPC_EAN_SAP { get; set; } = string.Empty;

        [Display(Name = "Restricted Ship States")]
        public string Restricted_States_Regulated { get; set; } = string.Empty;

        [Display(Name = "Color/Finish")]
        public string Color_Code_SAP { get; set; } = string.Empty;

        [Display(Name = "Color Swatch")]
        public string Color_Variant { get; set; } = string.Empty;

        [Display(Name = "Product Exclusive")]
        public string Customer_Exclusive_Material_YN { get; set; } = string.Empty;

        [Display(Name = "Moments of Truth")]
        public string Grohe_MOT { get; set; } = string.Empty;
    }
}
