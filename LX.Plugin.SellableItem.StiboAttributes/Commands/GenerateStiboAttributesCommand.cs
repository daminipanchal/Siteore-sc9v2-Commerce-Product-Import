using LX.Plugin.SellableItem.StiboAttributes.Pipelines;
using LX.Plugin.SellableItem.StiboAttributes.Pipelines.Arguments;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Core.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LX.Plugin.SellableItem.StiboAttributes.Commands
{
    public class GenerateStiboAttributesCommand : CommerceCommand
    {
        private readonly IGenerateStiboAttributesPipeline _pipeline;

        public GenerateStiboAttributesCommand(IGenerateStiboAttributesPipeline pipeline, IServiceProvider serviceProvider) : base(serviceProvider)
        {
            this._pipeline = pipeline;
        }

        public async Task<bool> Process(CommerceContext commerceContext, string catalogId)
        {
            try
            {
                using (var activity = CommandActivity.Start(commerceContext, this))
                {
                    var arg = new GenerateStiboAttributesArgument(catalogId);
                    var result = await this._pipeline.Run(arg, new CommercePipelineExecutionContextOptions(commerceContext));

                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
