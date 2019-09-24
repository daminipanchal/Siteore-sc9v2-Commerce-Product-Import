// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ImportSellableItemsCommandCommand.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2019
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace LX.Plugin.SellableItem.StiboAttributes.Commands
{
    using LX.Plugin.SellableItem.StiboAttributes.Pipelines;
    using LX.Plugin.SellableItem.StiboAttributes.Pipelines.Arguments;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Logging;
    using Sitecore.Commerce.Core;
    using Sitecore.Commerce.Core.Commands;
    using Sitecore.Commerce.Plugin.Catalog;
    using System;
    using System.IO;
    using System.Threading.Tasks;

    /// <inheritdoc />
    /// <summary>
    /// Defines the ImportSellableItemsCommandCommand command.
    /// </summary>
    public class ImportSellableItemsCommand : CommerceCommand
    {

        private readonly IIMportSellableItemsPipeline _pipeline;
        private readonly FindEntityCommand _findEntityCommand;
        private readonly GetCatalogCommand _getCatalogCommand;
        private readonly CreateCatalogCommand _createCatalogCommand;
        private readonly IPersistEntityPipeline _persistPipeline;
        private readonly IHostingEnvironment _hostingEnvironment;

        public ImportSellableItemsCommand(IIMportSellableItemsPipeline pipeline, IServiceProvider serviceProvider, FindEntityCommand findEntityCommand, 
            GetCatalogCommand getCatalogCommand, CreateCatalogCommand createCatalogCommand, IPersistEntityPipeline persistPipeline, IHostingEnvironment hostingEnvironment) : base(serviceProvider)
        {
            this._pipeline = pipeline;
            _findEntityCommand = findEntityCommand;
            _getCatalogCommand = getCatalogCommand;
            _createCatalogCommand = createCatalogCommand;
            _persistPipeline = persistPipeline;
            _hostingEnvironment = hostingEnvironment;
        }

        /// <summary>
        /// The process of the command
        /// </summary>
        /// <param name="commerceContext">
        /// The commerce context
        /// </param>
        /// <param name="parameter">
        /// The parameter for the command
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public async Task<string> Process(CommerceContext commerceContext)
        {
            using (var activity = CommandActivity.Start(commerceContext, this))
            {
                commerceContext.Logger.LogInformation($"ProductImport Started: Start Time: {DateTime.Now}");
                var context = commerceContext.PipelineContextOptions;
                ImportSellableItemPipelineArgument pArg = new ImportSellableItemPipelineArgument(Path.Combine(this._hostingEnvironment.WebRootPath, "ProductXMLs", "ProductImport.xml")); 
                var result = await this._pipeline.Run(pArg, context);
                commerceContext.Logger.LogInformation($"ProductImport ENDED: Start Time: {DateTime.Now}");
                return result.FileImportSuccess ? "Commerce SellableItems Imported Successfully" : "Error in Commerce SellableItems Import";
            }
        }
    }
}