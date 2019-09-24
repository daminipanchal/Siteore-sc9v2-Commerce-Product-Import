using Sitecore.Commerce.Core;

namespace LX.Plugin.SellableItem.StiboAttributes.Policies
{
    /// <inheritdoc />
    /// <summary>
    /// Defines a policy
    /// </summary>
    /// <seealso cref="T:Sitecore.Commerce.Core.Policy" />
    public class SellableItemStiboAttributesViewsPolicy : Policy
    {
        public string GlobalStiboAttributes { get; set; } = "GlobalStiboAttributes";
        public string ContentManageableAttributes { get; set; } = "ContentManageableAttributes";
        public string CategoryAttributes { get; set; } = "CategoryAttributes";
        public string StyleSpecification { get; set; } = "StyleSpecification";
        public string TechnicalSpecification { get; set; } = "TechnicalSpecification";
        public string AdditionalResources { get; set; } = "AdditionalResources";
        public string ProductFeatures { get; set; } = "ProductFeatures";
        public string ProductOverview { get; set; } = "ProductOverview";
        public string ProductVideoGallery { get; set; } = "ProductVideoGallery";
        public string InstallationAndInstructions { get; set; } = "InstallationAndInstructions";
        public string ProductMedia { get; set; } = "ProductMedia";
        public string OtherSpecifications { get; set; } = "OtherSpecifications";
        public string New { get; set; } = "New";
        public string Parts { get; set; } = "Parts";
        public string SellableItemFacets { get; set; } = "SellableItemFacets";
    }
}
