using Sitecore.Commerce.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LX.Plugin.SellableItem.StiboAttributes.Policies
{
    public class CategoryAttributesActionPolicy : Policy
    {
        public string CategoryAttributes { get; set; } = nameof(CategoryAttributes);
    }
}
