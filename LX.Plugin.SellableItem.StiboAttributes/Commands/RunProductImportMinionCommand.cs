namespace LX.Plugin.SellableItem.StiboAttributes.Commands
{
    using LX.Plugin.SellableItem.StiboAttributes.Pipelines;
    using Microsoft.Extensions.Logging;
    using Sitecore.Commerce.Core;
    using Sitecore.Commerce.Core.Commands;
    using System;
    using System.Threading.Tasks;

    //This is only to test Product imoport minion directly from postman
    //Note: We created this as there is one issue with calling minion. Minion gets executed twice. So, we are trying to call pipeline from command to test.
    public class RunProductImportMinionCommand : CommerceCommand
    {
        private readonly IIMportProductsMinionPipeline _pipeline;

        public RunProductImportMinionCommand(IIMportProductsMinionPipeline importProductMinionPipeline)
        {
            _pipeline = importProductMinionPipeline;
        }

        public async Task<string> Process(CommerceContext commerceContext)
        {
            using (var activity = CommandActivity.Start(commerceContext, this))
            {
                commerceContext.Logger.LogInformation($"ProductImport Minion Started: Start Time: {DateTime.Now}");
                var context = commerceContext.PipelineContextOptions;
                MinionRunResultsModel pArg = new MinionRunResultsModel();
                var result = await this._pipeline.Run(pArg, context);
                return "Success";
            }
        }
    }
}