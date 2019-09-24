// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ImportSellableItemPipelineArgumentArgument.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2019
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace LX.Plugin.SellableItem.StiboAttributes.Pipelines.Arguments
{

    using Sitecore.Commerce.Core;
    using Sitecore.Commerce.Plugin.Catalog;
    using Sitecore.Framework.Conditions;
    using System.Collections.Generic;


    /// <inheritdoc />
    /// <summary>
    /// The ImportSellableItemPipelineArgumentArgument.
    /// </summary>
    public class ImportSellableItemPipelineArgument : PipelineArgument
    {
        public ImportSellableItemPipelineArgument(string xmlfilepath)
        {
            Condition.Requires(xmlfilepath).IsNotNull("The xmlfilepath parameter can not be null");
            this.XMlFilePath = xmlfilepath;
        }
        public string XMlFilePath { get; set; }
        public string XMlFileName { get; set; }
        public bool IsXMlfileValidate { get; set; }
        public Catalog Catalog { get; set; }
        public string CatalogName { get; set; }
        public string CatalogDisplayName { get; set; }
        public bool FileImportSuccess { get; set; }
        public List<string> ProductImportStatusList { get; set; }
        public List<string> ProductSuccessList { get; set; }
        public List<string> ProductFailureList { get; set; }
        public List<string> ProductUpdateList { get; set; }
        public List<string> VariantUpdateList { get; set; }
        public List<string> CategorySuccessList { get; set; }
        public List<string> VariantSuccessList { get; set; }
        public List<string> VariantFailureList { get; set; }
        public List<string> CategoryFailureList { get; set; }
    }
}