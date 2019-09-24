using LX.Plugin.SellableItem.StiboAttributes.Pipelines.Arguments;
using Sitecore.Commerce.Core;
using Sitecore.Framework.Pipelines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LX.Plugin.SellableItem.StiboAttributes.Pipelines
{
    [PipelineDisplayName("GenerateStiboAttributesPipeline")]
    public interface IGenerateStiboAttributesPipeline : IPipeline<GenerateStiboAttributesArgument, bool, CommercePipelineExecutionContext>
    {
    }
}
