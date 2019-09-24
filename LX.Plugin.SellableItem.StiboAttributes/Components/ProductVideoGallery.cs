using Sitecore.Commerce.Core;
using System.ComponentModel.DataAnnotations;

namespace LX.Plugin.SellableItem.StiboAttributes.Components
{
    public class ProductVideoGallery : Component
    {
        [Display(Name = "Additional Videos")]
        public string AdditionalVideoURLs { get; set; } = string.Empty;

        [Display(Name = "Installation Video")]
        public string Video_Install_URL { get; set; } = string.Empty;
    }
}
