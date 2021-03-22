using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace DurableFunctions
{
    public static class EscalationActivityFunction
    {

        [FunctionName("EscalationActivityFunction")]
        public static string SayHello([ActivityTrigger] string name, ILogger log)
        {
            return $"ESCALATION : You have not approved the project design proposal - reassigning to your Manager! {name}!";
        }
    }
}