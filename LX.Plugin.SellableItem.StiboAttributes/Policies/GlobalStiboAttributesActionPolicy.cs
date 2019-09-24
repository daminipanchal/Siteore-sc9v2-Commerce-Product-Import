using Sitecore.Commerce.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LX.Plugin.SellableItem.StiboAttributes.Policies
{
    public class GlobalStiboAttributesActionPolicy : Policy
    {
        public string GlobalStiboAttributes { get; set; } = nameof(GlobalStiboAttributes);
    }
}
