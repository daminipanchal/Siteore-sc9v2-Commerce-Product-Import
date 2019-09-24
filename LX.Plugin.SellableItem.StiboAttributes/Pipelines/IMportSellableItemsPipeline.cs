

namespace LX.Plugin.SellableItem.StiboAttributes.Pipelines
{
    using LX.Plugin.SellableItem.StiboAttributes.Pipelines.Arguments;
    using Microsoft.Extensions.Logging;
    using Sitecore.Commerce.Core;
    using Sitecore.Framework.Pipelines;


    public class IMportSellableItemsPipeline : CommercePipeline<ImportSellableItemPipelineArgument, ImportSellableItemPipelineArgument>,IIMportSellableItemsPipeline
    {
        public IMportSellableItemsPipeline(IPipelineConfiguration<IIMportSellableItemsPipeline> configuration, ILoggerFactory loggerFactory)
            : base(configuration, loggerFactory)
        {
        }
    }
}

