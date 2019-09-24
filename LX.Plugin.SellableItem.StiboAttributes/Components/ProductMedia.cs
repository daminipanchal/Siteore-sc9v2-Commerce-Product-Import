using Sitecore.Commerce.Core;
using System.ComponentModel.DataAnnotations;

namespace LX.Plugin.SellableItem.StiboAttributes.Components
{
    public class ProductMedia : Component
    {
        [Display(Name = "Main Image")]
        public string Main_Image_URL_2000 { get; set; } = string.Empty;

        [Display(Name = "Lifestyle Images")]
        public string Lifestyle_Image_Address { get; set; } = string.Empty;

        [Display(Name = "Infographic")]
        public string Infographic_URL { get; set; } = string.Empty;

        [Display(Name = "Features & Benefits Video")]
        public string Video_F_B_URL { get; set; } = string.Empty;
    }
}
