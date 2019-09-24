// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PipelineBlock1Block.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2019
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace LX.Plugin.SellableItem.StiboAttributes.Pipelines.Blocks.Import
{

    using LX.Plugin.SellableItem.StiboAttributes.Pipelines.Arguments;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Logging;
    using Sitecore.Commerce.Core;
    using Sitecore.Framework.Conditions;
    using Sitecore.Framework.Pipelines;
    using System.IO;
    using System.Threading.Tasks;


    [PipelineDisplayName(ImportSellableItemsConstants.Pipelines.Blocks.ValidateXMLExists)]
    public class ValidateXMLFile : PipelineBlock<ImportSellableItemPipelineArgument, ImportSellableItemPipelineArgument, CommercePipelineExecutionContext>
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        public ValidateXMLFile(IHostingEnvironment hostingEnvironment)
        {
            this._hostingEnvironment = hostingEnvironment;
        }

        public override Task<ImportSellableItemPipelineArgument> Run(ImportSellableItemPipelineArgument arg, CommercePipelineExecutionContext context)
        {
            context.Logger.LogInformation("---------ValidateXMLExists started------------");
            context.Logger.LogInformation($"ValidateXMLExists _hostingEnvironment path : {_hostingEnvironment.WebRootPath}");
            context.Logger.LogInformation($"ValidateXMLExists arg.XMlFilePath path : {arg.XMlFilePath}");

            return Task.Run(() =>
            {
                string filePath = arg.XMlFilePath;
                context.Logger.LogInformation($"ValidateXMLExists filePath  : {filePath}");
                if (File.Exists(filePath))
                {
                    context.Logger.LogInformation($"ValidateXMLExists XML Exists at path : {filePath}");
                    arg.IsXMlfileValidate = true;
                    arg.XMlFileName = Path.GetFileName(arg.XMlFilePath);
                    arg.XMlFilePath = filePath;
                }
                context.Logger.LogInformation("---------ValidateXMLExists completed------------");
                return arg;
            });
        }
    }
}