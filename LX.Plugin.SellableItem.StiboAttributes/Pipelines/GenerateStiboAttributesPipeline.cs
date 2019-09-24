using LX.Plugin.SellableItem.StiboAttributes.Pipelines.Arguments;
using Microsoft.Extensions.Logging;
using Sitecore.Commerce.Core;
using Sitecore.Framework.Pipelines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LX.Plugin.SellableItem.StiboAttributes.Pipelines
{
    public class GenerateStiboAttributesPipeline : CommercePipeline<GenerateStiboAttributesArgument, bool>, IGenerateStiboAttributesPipeline
    {
        public GenerateStiboAttributesPipeline(IPipelineConfiguration<IGenerateStiboAttributesPipeline> configuration, ILoggerFactory loggerFactory)
            : base(configuration, loggerFactory)
        {
        }
    }
}

