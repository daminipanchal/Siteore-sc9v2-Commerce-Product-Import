using Microsoft.Extensions.Logging;
using Sitecore.Commerce.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LX.Plugin.SellableItem.StiboAttributes.Minions
{
    public abstract class SerialMinion : Minion
    {
        public override async Task StartAsync()
        {
            await Task.Run(async () =>
               {
                   while (true)
                   {
                       try
                       {
                           await this.Execute();
                       }
                       catch
                       {
                        // Prevent exceptions from breaking the loop
                        // (not logged since the base implementation doesn't either)
                    }

                       if (this.Policy.WakeupInterval.HasValue)
                       {
                           await Task.Delay(this.Policy.WakeupInterval.Value);
                       }
                   }
               }).ConfigureAwait(false);
        }
    }

    public abstract class NonReentrantMinion : Minion
    {
        private int isRunning = 0;

        public abstract Task<MinionRunResultsModel> SafeRun();

        protected override async Task<MinionRunResultsModel> Execute()
        {
            if (Interlocked.CompareExchange(ref isRunning, 1, 0) != 0)
            {
                // Log skipped invocation
                return new MinionRunResultsModel();
            }

            try
            {
                return await SafeRun();
            }
            finally
            {
                isRunning = 0;
            }
        }
    }
}
