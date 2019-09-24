

namespace LX.Plugin.SellableItem.StiboAttributes.Pipelines
{
    using LX.Plugin.SellableItem.StiboAttributes.Pipelines.Arguments;
    using Microsoft.Extensions.Logging;
    using Sitecore.Commerce.Core;
    using Sitecore.Framework.Pipelines;


    public class IMportProductsMinionPipeline : CommercePipeline<MinionRunResultsModel, MinionRunResultsModel>, IIMportProductsMinionPipeline
    {
        public IMportProductsMinionPipeline(IPipelineConfiguration<IIMportProductsMinionPipeline> configuration, ILoggerFactory loggerFactory)
            : base(configuration, loggerFactory)
        {
        }
    }

}

