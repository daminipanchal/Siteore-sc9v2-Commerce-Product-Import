using Sitecore.Commerce.Core;

namespace LX.Plugin.SellableItem.StiboAttributes.Policies
{


    /// <inheritdoc />
    /// <summary>
    /// Defines a policy
    /// </summary>
    /// <seealso cref="T:Sitecore.Commerce.Core.Policy" />
    public class SellableItemStiboAttributesActionsPolicy : Policy
    {

        public string ProductMedia { get; set; } = nameof(ProductMedia);
        public string GlobalStiboAttributes { get; set; } = nameof(GlobalStiboAttributes);
        public string CategoryAttributes { get; set; } = nameof(CategoryAttributes);
        public string ContentManageableAttributes { get; set; } = nameof(ContentManageableAttributes);
        public string StyleSpecification { get; set; } = nameof(StyleSpecification);
        public string TechnicalSpecification { get; set; } = nameof(TechnicalSpecification);
        public string AdditionalResources { get; set; } = nameof(AdditionalResources);
        public string ProductVideoGallery { get; set; } = nameof(ProductVideoGallery);
        public string InstallationAndInstructions { get; set; } = nameof(InstallationAndInstructions);
        public string OtherSpecifications { get; set; } = nameof(OtherSpecifications);
        public string New { get; set; } = nameof(New);
        public string Parts { get; set; } = nameof(Parts);
        public string SellableItemFacets { get; set; } = nameof(SellableItemFacets);
        public string ProductFeatures { get; set; } = nameof(ProductFeatures);
        public string ProductOverview { get; set; } = nameof(ProductOverview);

    }
}
