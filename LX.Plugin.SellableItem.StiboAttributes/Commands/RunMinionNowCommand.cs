using Sitecore.Commerce.Core;
using Sitecore.Commerce.Core.Commands;
using Sitecore.Framework.Pipelines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LX.Plugin.SellableItem.StiboAttributes.Commands
{
    public class RunMinionNowCommand : CommerceCommand
    {

        private readonly IRunMinionPipeline _runMinionPipeline;
        private readonly GetEnvironmentCommand _getEnvironment;

        public RunMinionNowCommand(IRunMinionPipeline runMinionPipeline, IServiceProvider serviceProvider, GetEnvironmentCommand getEnvironmentPipeline)
        : base(serviceProvider)
        {
            this._runMinionPipeline = runMinionPipeline;
            this._getEnvironment = getEnvironmentPipeline;
        }

        public virtual async Task<RunMinionNowCommand> Process(CommerceContext commerceContext, string minionFullName, string environmentName, IList<Policy> policies)
        {
            RunMinionNowCommand runMinionNowCommand = this;

            using (CommandActivity.Start(commerceContext, (CommerceCommand)runMinionNowCommand))
            {
                if (policies == null)
                    policies = (IList<Policy>)new List<Policy>();
                CommerceEnvironment commerceEnvironment = await this._getEnvironment.Process(commerceContext, environmentName) ?? commerceContext.Environment;
                CommercePipelineExecutionContextOptions pipelineContextOptions = commerceContext.PipelineContextOptions;
                pipelineContextOptions.CommerceContext.Environment = commerceEnvironment;
                IRunMinionPipeline runMinionPipeline = this._runMinionPipeline;
                RunMinionArgument runMinionArgument = new RunMinionArgument(minionFullName);
                runMinionArgument.Policies = policies;
                CommercePipelineExecutionContextOptions executionContextOptions = pipelineContextOptions;
                int num = await runMinionPipeline.Run(runMinionArgument, (IPipelineExecutionContextOptions)executionContextOptions) ? 1 : 0;
            }
            return runMinionNowCommand;

        }
    }
}
