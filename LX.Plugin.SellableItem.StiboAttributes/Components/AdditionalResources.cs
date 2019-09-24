using Sitecore.Commerce.Core;
using System.ComponentModel.DataAnnotations;

namespace LX.Plugin.SellableItem.StiboAttributes.Components
{
    public class AdditionalResources : Component
    {
        [Display(Name = "CAD Drawings 2D")]
        public string CAD_2D_URL { get; set; } = string.Empty;

        [Display(Name = "CAD Drawings 3D")]
        public string CAD_3D_URL { get; set; } = string.Empty;

        [Display(Name = "Spray Pattern")]
        public string Spec_Sheet_URL { get; set; } = string.Empty;
    }
}
