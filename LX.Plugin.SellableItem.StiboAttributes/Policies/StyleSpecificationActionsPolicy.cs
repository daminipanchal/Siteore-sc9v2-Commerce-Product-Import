﻿using Sitecore.Commerce.Core;

namespace LX.Plugin.SellableItem.StiboAttributes.Policies
{
    /// <inheritdoc />
    /// <summary>
    /// Defines a policy
    /// </summary>
    /// <seealso cref="T:Sitecore.Commerce.Core.Policy" />
    public class StyleSpecificationActionsPolicy :Policy
    {
        public string StyleSpecification { get; set; } = nameof(StyleSpecification);
    }
}
