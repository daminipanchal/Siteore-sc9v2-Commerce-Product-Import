using System;
using Sitecore.Commerce.Core;
using System.ComponentModel.DataAnnotations;

namespace LX.Plugin.SellableItem.StiboAttributes.Components
{
    public class New : Component
    {
        [Display(Name = "US Launch Date(YYYY-MM-DD)")]
        public DateTime US_Launch_Date_MM_DD_YYYY { get; set; }
    }
}
