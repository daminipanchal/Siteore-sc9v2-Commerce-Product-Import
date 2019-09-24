﻿using Sitecore.Commerce.Core;

namespace LX.Plugin.SellableItem.StiboAttributes.Policies
{
    /// <inheritdoc />
    /// <summary>
    /// Defines a policy
    /// </summary>
    /// <seealso cref="T:Sitecore.Commerce.Core.Policy" />
    public class AdditionalResourcesActionsPolicy : Policy
    {
        public string AdditionalResources { get; set; } = nameof(AdditionalResources);
    }
}
