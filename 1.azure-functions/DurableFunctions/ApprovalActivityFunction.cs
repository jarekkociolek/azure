using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;

namespace DurableFunctions
{
    public static class ApprovalActivityFunction
    {

        [FunctionName("ApprovalActivityFunction")]
        public static string SayHello([ActivityTrigger] string name, ILogger log)
        {
            return $"Your project design proposal has been {name}!";
        }
    }
}