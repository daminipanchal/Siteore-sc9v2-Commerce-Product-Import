using Sitecore.Commerce.Core;
using System;
using System.ComponentModel.DataAnnotations;

namespace LX.Plugin.SellableItem.StiboAttributes.Components
{
    /// <summary>
    /// The SellableItemStiboAttributesComponent.
    /// </summary>
    public class GlobalStiboAttributes : Component
    {
        [Display(Name = "SAP SKU Status SAP")]
        public string SAP_SKU_Status_SAP { get; set; } = string.Empty;

        [Display(Name = "Base UOM SAP")]
        public string Base_UOM_SAP { get; set; } = string.Empty;

        [Display(Name = "Number of Items")]
        public int AltUOMConvFac { get; set; } = 0;

        [Display(Name = "Is Discontinued")]
        public bool Is_Discontinued { get; set; } = false;

        [Display(Name = "SKU Status")]
        public string SKU_Status { get; set; } = string.Empty;

        [Display(Name = "Product Status")]
        public string Product_Status { get; set; } = string.Empty;

        [Display(Name = "Quantity")]
        public int Stock_Level_SAP { get; set; } = 0;

        [Display(Name = "LTL")]
        public string Small_Parcel_Postable_SAP_YN { get; set; } = string.Empty;

        [Display(Name = "Product Category")]
        public string SAPLink { get; set; } = string.Empty;

        [Display(Name = "DC Location")]
        public string DC_Location { get; set; } = string.Empty;

        [Display(Name = "Made to Order/ Made to Ship")]
        public string MTO_or_MTS { get; set; } = string.Empty;

        [Display(Name = "Model Number")]
        public string ParentNumber { get; set; } = string.Empty;

        [Display(Name = "Number of Boxes")]
        public string Pkg_Component_1 { get; set; } = string.Empty;

        [Display(Name = "DC Location - Shipping Plants")]
        public string Shipping_Plants_SAP { get; set; } = string.Empty;

        [Display(Name = "Shower No. of Showerheads")]
        public int Shower_No_Of_Showerheads { get; set; } = 0;
        [Display(Name = "Hand Shower Included")]
        public string Hand_Shower_Included_YN { get; set; } = string.Empty;

        [Display(Name = "Bath Accessory Application")]
        public string Bath_Accessory_Application { get; set; } = string.Empty;
        [Display(Name = "No. of Sink Pieces")]
        public int Sink_No_Of_Pieces { get; set; } = 0;
    }
}

