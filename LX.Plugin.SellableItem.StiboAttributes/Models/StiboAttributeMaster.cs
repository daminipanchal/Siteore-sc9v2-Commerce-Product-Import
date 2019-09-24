using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LX.Plugin.SellableItem.StiboAttributes.Models
{
    public class StiboAttributeMaster
    {
        public string StiboName { get; set; }
        public string ValueType { get; set; }
        public string TransformedValue { get; set; }
        public string AttributeLevel { get; set; }
    }
}
