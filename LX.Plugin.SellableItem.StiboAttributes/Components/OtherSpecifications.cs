using Sitecore.Commerce.Core;
using System.ComponentModel.DataAnnotations;

namespace LX.Plugin.SellableItem.StiboAttributes.Components
{
    public class OtherSpecifications : Component
    {
        [Display(Name = "Each Weight Gross Weight SAP")]
        public decimal Each_Weight_Gross_Weight_SAP { get; set; } = 0;

        [Display(Name = "Warranty Type")]
        public string Warranty_Type { get; set; } = string.Empty;

        [Display(Name = "UN NA Number")]
        public string UN_NA_Number { get; set; } = string.Empty;

        [Display(Name = "UNSPSC Code United Nations Code")]
        public string UNSPSC_Code_United_Nations_Code { get; set; } = string.Empty;

        [Display(Name = "NFPA Storage Classification")]
        public string NFPA_Storage_Classification { get; set; } = string.Empty;

        [Display(Name = "OMRD")]
        public string ORMD_YN { get; set; } = string.Empty;

        [Display(Name = "Hazmat")]
        public string Hazmat_YN { get; set; } = string.Empty;

        [Display(Name = "Hazard Class Number")]
        public string Hazard_Class_Number { get; set; } = string.Empty;

        [Display(Name = "Kit")]
        public string Kit_SMO_POD { get; set; } = string.Empty;

        [Display(Name = "Part Number")]
        public string Mfg_Part_Number_SAP { get; set; } = string.Empty;

        [Display(Name = "Kitchen or Bathroom Product")]
        public string Kitchen_Bath { get; set; } = string.Empty;

        [Display(Name = "Retail Price")]
        public decimal Retail_Price { get; set; } = 0;

        [Display(Name = "Bowl Dimensions")]
        public int Bowl_Front_To_Back_Length { get; set; } = 0;

        [Display(Name = "Residential or Commercial")]
        public string Residential_Or_Commercial { get; set; } = string.Empty;

        [Display(Name = "Battery Lifetime Cycles")]
        public string Battery_Lifetime_Cycles { get; set; } = string.Empty;

        [Display(Name = "Batteries Required")]
        public string Batteries_Required { get; set; } = string.Empty;

        [Display(Name = "Included In Box")]
        public string Included_In_Box { get; set; } = string.Empty;

        [Display(Name = "Freestanding Tub Style")]
        public string Freestanding_Tub_Style { get; set; } = string.Empty;

        [Display(Name = "Faucet Included")]
        public string Faucet_Included_YN { get; set; } = string.Empty;

        [Display(Name = "Removable")]
        public string Removable_YN { get; set; } = string.Empty;

        [Display(Name = "Is Sellable")]
        public bool Is_Sellable { get; set; } = false;
    }
}
