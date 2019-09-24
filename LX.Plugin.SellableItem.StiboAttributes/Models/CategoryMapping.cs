using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LX.Plugin.SellableItem.StiboAttributes.Models
{
    public class CategoryMapping
    {
        public Sitecore.Commerce.Plugin.Catalog.Category Category { get; set; }

        public string CategoryMappingPath { get; set; } 
    }
}
