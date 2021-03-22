using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using System;
using System.Threading;
using Microsoft.Extensions.Logging;

namespace DurableFunctions
{
    public static class OrchestrationFunction
    {
        [FunctionName("OrchestrationFunction")]
        public static async Task<List<string>> RunOrchestrator(
            [OrchestrationTrigger] IDurableOrchestrationContext context, ILogger log)
        {
            var outputs = new List<string>();

            var deadline = context.CurrentUtcDateTime.Add(TimeSpan.FromSeconds(10));

            using (var cts = new CancellationTokenSource())
            {
                Task timeoutTask = context.CreateTimer(deadline, cts.Token);
                Task approvalTask = context.WaitForExternalEvent<string>("ApprovalActivityFunction");

                var winner = await Task.WhenAny(timeoutTask, approvalTask);

                if (winner == timeoutTask)
                {
                    outputs.Add(await context.CallActivityAsync<string>("EscalationActivityFunction", "John"));
                }
                else
                {
                    cts.Cancel();
                    outputs.Add(await context.CallActivityAsync<string>("ApprovalActivityFunction", "Approved"));
                }
            }

            return outputs;
        }
    }
}