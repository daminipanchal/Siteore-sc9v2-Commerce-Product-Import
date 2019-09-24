using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LX.Plugin.SellableItem.StiboAttributes.Models
{
    public class Category
    {
        public string Commerce_Name { get; set; }
        public string StiboClassifications { get; set; }
        public List<ProductClassification> ClassificationsItems { get; set; }

        public string ItemPath { get; set; }
    }
}
