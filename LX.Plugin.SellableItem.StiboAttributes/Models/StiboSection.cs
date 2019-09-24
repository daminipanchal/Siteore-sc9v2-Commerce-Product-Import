using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LX.Plugin.SellableItem.StiboAttributes.Models
{
    public class StiboSection
    {
        public string Section_Name { get; set; }
        public string Stibo_Attributes { get; set; }
        public List<StiboAttributeMaster> StiboAttributeList { get; set; }
    }
}
