using Sitecore.Commerce.Core;

namespace LX.Plugin.SellableItem.StiboAttributes.Policies
{
    /// <inheritdoc />
    /// <summary>
    /// Defines a policy
    /// </summary>
    /// <seealso cref="T:Sitecore.Commerce.Core.Policy" />
    public class SellableItemFacetsActionsPolicy : Policy
    {
        public string SellableItemFacets { get; set; } = nameof(SellableItemFacets);
    }
}
