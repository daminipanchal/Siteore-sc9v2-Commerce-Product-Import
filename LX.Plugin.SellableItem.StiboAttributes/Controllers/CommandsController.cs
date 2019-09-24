// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CommandsController.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2019
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace LX.Plugin.SellableItem.StiboAttributes.Controllers
{
    using LX.Plugin.SellableItem.StiboAttributes.Commands;
    using Microsoft.AspNetCore.Mvc;
    using Newtonsoft.Json.Linq;
    using Sitecore.Commerce.Core;
    using Sitecore.Commerce.Core.Commands;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Web.Http.OData;

    /// <inheritdoc />
    /// <summary>
    /// Defines a controller
    /// </summary>
    /// <seealso cref="T:Sitecore.Commerce.Core.CommerceController" />
    public class CommandsController : CommerceController
    {
        public CommandsController(IServiceProvider serviceProvider, CommerceEnvironment globalEnvironment)
            : base(serviceProvider, globalEnvironment)
        {
            
        }
        [HttpPut]
        [Route("ImportSellableItems()")]
        public async Task<IActionResult> ImportSellableItems()
        {
            var command = this.Command<ImportSellableItemsCommand>();
            var result = await command.Process(this.CurrentContext);
            return new ObjectResult(command);
        }

        [HttpPut]
        [Route("RunProductImportMinion()")]
        public async Task<IActionResult> RunProductImportMinion()
        {
            var command = this.Command<RunProductImportMinionCommand>();
            var result = await command.Process(this.CurrentContext);
            return new ObjectResult(command);
        }

        [HttpPut]
        [Route("RunMinionNow()")]
        public async Task<IActionResult> RunMinionNow([FromBody] ODataActionParameters value)
        {
            CommandsController commandsController = this;
            if (!commandsController.ModelState.IsValid || value == null)
                return (IActionResult)new BadRequestObjectResult(commandsController.ModelState);
            if (value.ContainsKey("minionFullName") && value.ContainsKey("environmentName"))
            {
                var minionFullName = value["minionFullName"].ToString();
                var environmentName = value["environmentName"].ToString();
                if ((!string.IsNullOrEmpty(minionFullName != null ? minionFullName.ToString() : (string)null)) && (!string.IsNullOrEmpty(environmentName != null ? environmentName.ToString() : (string)null)))
                {

                    List<Policy> policies = new List<Policy>();
                    RunMinionNowCommand command = commandsController.Command<RunMinionNowCommand>();
                    var resutlt = await command.Process(commandsController.CurrentContext, minionFullName, environmentName, policies);
                    return (IActionResult)new ObjectResult((object)command);
                }
            }
            return (IActionResult)new BadRequestObjectResult((object)value);
        }
    }
}