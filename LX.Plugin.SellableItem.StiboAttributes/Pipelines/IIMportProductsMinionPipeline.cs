

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IIMportSellableItemsPipelineArgument.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2019
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace LX.Plugin.SellableItem.StiboAttributes.Pipelines
{
    using LX.Plugin.SellableItem.StiboAttributes.Pipelines.Arguments;
    using Sitecore.Commerce.Core;
    using Sitecore.Framework.Pipelines;

    /// <inheritdoc />
    /// <summary>
    /// The IIMportSellableItemsPipelineArgument.
    /// </summary>
    /// 

    [PipelineDisplayName(ImportSellableItemsConstants.Pipelines.IIMportProductsMinionPipeline)]
    public interface IIMportProductsMinionPipeline : IPipeline<MinionRunResultsModel, MinionRunResultsModel, CommercePipelineExecutionContext>
    {

    }
}