using Sitecore.Commerce.Core;
using System.ComponentModel.DataAnnotations;

namespace LX.Plugin.SellableItem.StiboAttributes.Components
{
    public class InstallationAndInstructions : Component
    {
        [Display(Name = "Installation Sheets")]
        public string Installation_Instruction_URL { get; set; } = string.Empty;

        [Display(Name = "Installation Type - Kitchen Sink")]
        public string Kitchen_Sink_Mount_Location { get; set; } = string.Empty;

        [Display(Name = "Warranty PDF URL")]
        public string Warranty_PDF_URL { get; set; } = string.Empty;

        [Display(Name = "Dimensional Diagram")]
        public string Dimensional_Drawing_URL { get; set; } = string.Empty;

        [Display(Name = "Enviornmental Product Declaration")]
        public string EPD_URL { get; set; } = string.Empty;

        [Display(Name = "User Instruction Manual")]
        public string User_Instr_PDF_URL { get; set; } = string.Empty;

        [Display(Name = "Use and Care")]
        public string Use_and_Care_URL { get; set; } = string.Empty;

        [Display(Name = "Safety Data Sheet(SDS or MSDS) URL")]
        public string MSDS_URL { get; set; } = string.Empty;
    }
}
