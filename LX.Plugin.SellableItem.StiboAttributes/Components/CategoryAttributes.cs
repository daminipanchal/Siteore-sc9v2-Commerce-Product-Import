using System;
using Sitecore.Commerce.Core;
using System.ComponentModel.DataAnnotations;

namespace LX.Plugin.SellableItem.StiboAttributes.Components
{
    public class CategoryAttributes : Component
    {
        [Display(Name = "Sitecore Mapping Path")]
        public string SitecoreMappingPath { get; set; } = string.Empty;

        [Display(Name = "Classification Type")]
        public string ClassificationType { get; set; } = string.Empty;
    }
}
