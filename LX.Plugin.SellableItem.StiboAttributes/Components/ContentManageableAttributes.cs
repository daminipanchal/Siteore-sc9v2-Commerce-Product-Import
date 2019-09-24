using Sitecore.Commerce.Core;
using System.ComponentModel.DataAnnotations;

namespace LX.Plugin.SellableItem.StiboAttributes.Components
{
    public class ContentManageableAttributes : Component
    {
        [Display(Name = "Model Number")]
        public string Mfg_Product_Number_SAP { get; set; } = string.Empty;

        [Display(Name = "Collection")]
        public string Product_Family_SAP { get; set; } = string.Empty;

        [Display(Name = "Product Name")]
        public string Material_Description_Marketing { get; set; } = string.Empty;

        [Display(Name = "Product Description")]
        public string Marketing_Copy { get; set; } = string.Empty;

        [Display(Name = "Products Using This Part")]
        public string Products_Use_This_Part { get; set; } = string.Empty;

        [Display(Name = "Related Products")]
        public string Related_Products { get; set; } = string.Empty;

        [Display(Name = "Lead Law Compliant")]
        public string Lead_Law_Compliant_YN { get; set; } = string.Empty;
    }
}
