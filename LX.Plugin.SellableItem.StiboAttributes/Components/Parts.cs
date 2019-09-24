using Sitecore.Commerce.Core;
using System.ComponentModel.DataAnnotations;

namespace LX.Plugin.SellableItem.StiboAttributes.Components
{
    public class Parts : Component
    {
        [Display(Name = "Replacement Parts")]
        public string Replacement_Material { get; set; } = string.Empty;

        public string Replacement_Material_URL { get; set; } = string.Empty;

        [Display(Name = "Required Parts")]
        public string Replacement_Parts { get; set; } = string.Empty;

        public string Replacement_Parts_URL { get; set; } = string.Empty;

        [Display(Name = "Included Components")]
        public string Included_Components { get; set; } = string.Empty;

        [Display(Name = "Replacement Parts Diagram")]
        public string Replacement_Parts_Doc_URL { get; set; } = string.Empty;
    }
}
