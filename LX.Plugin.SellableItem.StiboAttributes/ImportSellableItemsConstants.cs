using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LX.Plugin.SellableItem.StiboAttributes
{
    public static class ImportSellableItemsConstants
    {
        public class Pipelines
        {
            public const string ImportSellableItems = "ImportSellableItems:pipelines:importsellableitems";
            public const string IIMportProductsMinionPipeline = "ImportSellableItems:Minion:pipelines:importsellableitems";
            
            public class Blocks
            {
                public const string ValidateXMLExists = "ImportSellableItems:blocks:validatexmlfile";
                public const string CatalogManager = "ImportSellableItems:blocks:catalogmanager";
                public const string SellableItemManager = "ImportSellableItems:blocks:sellableitemmanager";
                public const string ManageProductImportStatus = "ImportSellableItems:blocks:manageproductimportstatus";
            }
        }      
    }
}
