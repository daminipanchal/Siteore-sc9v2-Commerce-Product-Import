// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConfigureServiceApiBlock.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2017
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace LX.Plugin.SellableItem.StiboAttributes
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.OData.Builder;

    using Sitecore.Commerce.Core;
    using Sitecore.Commerce.Core.Commands;
    using Sitecore.Framework.Conditions;
    using Sitecore.Framework.Pipelines;

    /// <summary>
    /// Defines a block which configures the OData model
    /// </summary>
    /// <seealso>
    ///     <cref>
    ///         Sitecore.Framework.Pipelines.PipelineBlock{Microsoft.AspNetCore.OData.Builder.ODataConventionModelBuilder,
    ///         Microsoft.AspNetCore.OData.Builder.ODataConventionModelBuilder,
    ///         Sitecore.Commerce.Core.CommercePipelineExecutionContext}
    ///     </cref>
    /// </seealso>
    [PipelineDisplayName(SellableItemStiboAttributesConstants.Pipelines.SellableItemStiboAttributesPipelineName)]
    public class ConfigureServiceApiBlock : PipelineBlock<ODataConventionModelBuilder, ODataConventionModelBuilder, CommercePipelineExecutionContext>
    {
        /// <summary>
        /// The execute.
        /// </summary>
        /// <param name="modelBuilder">
        /// The argument.
        /// </param>
        /// <param name="context">
        /// The context.
        /// </param>
        /// <returns>
        /// The <see cref="ODataConventionModelBuilder"/>.
        /// </returns>
        public override Task<ODataConventionModelBuilder> Run(ODataConventionModelBuilder modelBuilder, CommercePipelineExecutionContext context)
        {
            Condition.Requires(modelBuilder).IsNotNull($"{this.Name}: The argument cannot be null.");
            // Add unbound actions
            var configuration = modelBuilder.Action("ImportSellableItems"); configuration.ReturnsFromEntitySet<CommerceCommand>("Commands");
            var configuration1 = modelBuilder.Action("RunProductImportMinion"); configuration1.ReturnsFromEntitySet<CommerceCommand>("Commands");

            ActionConfiguration configuration2 = modelBuilder.Action("RunMinionNow");
            configuration2.Parameter<string>("minionFullName");
            configuration2.Parameter<string>("environmentName");
            configuration2.ReturnsFromEntitySet<CommerceCommand>("Commands");

            return Task.FromResult(modelBuilder);
        }
    }
}
