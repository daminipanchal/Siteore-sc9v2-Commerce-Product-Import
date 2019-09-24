using Sitecore.Commerce.Core;
using Sitecore.Framework.Conditions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LX.Plugin.SellableItem.StiboAttributes.Pipelines.Arguments
{
    public class GenerateStiboAttributesArgument : PipelineArgument
    {

        public GenerateStiboAttributesArgument(string catalogId)
        {
            Condition.Requires(catalogId).IsNotNull("The parameter can not be null");

            this.CatalogId = catalogId;
        }

        public string CatalogId { get; set; }
    }
}
