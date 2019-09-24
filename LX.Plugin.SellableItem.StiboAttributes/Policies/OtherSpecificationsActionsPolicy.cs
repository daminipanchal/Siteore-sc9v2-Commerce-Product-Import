using Sitecore.Commerce.Core;

namespace LX.Plugin.SellableItem.StiboAttributes.Policies
{
    /// <inheritdoc />
    /// <summary>
    /// Defines a policy
    /// </summary>
    /// <seealso cref="T:Sitecore.Commerce.Core.Policy" />
    public class OtherSpecificationsActionsPolicy : Policy
    {
        public string OtherSpecifications { get; set; } = nameof(OtherSpecifications);
    }
}
