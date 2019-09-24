using Sitecore.Commerce.Core;
using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using LX.Plugin.SellableItem.StiboAttributes.Pipelines;
using LX.Plugin.SellableItem.StiboAttributes.Pipelines.Arguments;

namespace LX.Plugin.SellableItem.StiboAttributes.Minions
{
    public class ImportProductsMinion : Minion
    {
        protected IIMportProductsMinionPipeline _iImportProuctsMinionPipeline { get; set; }

        [Obsolete]
        public override void Initialize(IServiceProvider serviceProvider,
            ILogger logger,
            MinionPolicy policy,
            CommerceEnvironment environment,
            CommerceContext globalContext)
        {
            base.Initialize(serviceProvider, logger, policy, environment, globalContext);
            _iImportProuctsMinionPipeline = serviceProvider.GetService<IIMportProductsMinionPipeline>();
        }

        public override Task StartAsync()
        {
            this.Logger.LogInformation(this.Name + " - ImportProduct minions do not auto start");
            return (Task)null;
        }

        protected override async Task<MinionRunResultsModel> Execute()
        {
            this.Logger.LogInformation("ImportProduct minion started");
            MinionRunResultsModel runResults = new MinionRunResultsModel();

            var commerceContext = new CommerceContext(this.Logger, this.MinionContext.TelemetryClient, (IGetLocalizableMessagePipeline)null);
            commerceContext.Environment = this.Environment;
            CommercePipelineExecutionContextOptions executionContextOptions = new CommercePipelineExecutionContextOptions(commerceContext, null, null, null, null, null);

            try
            {
                runResults = await _iImportProuctsMinionPipeline.Run(runResults, executionContextOptions);
            }
            finally
            {
                runResults.HasMoreItems = false;
                runResults.DidRun = true;
            }

            this.Logger.LogInformation("ImportProduct minion completed");
            return runResults;
        }
    }
}
