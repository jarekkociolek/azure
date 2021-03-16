using System.IO;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace FunctionApp
{
    public static class BlobTriggerFunction
    {
        [FunctionName("BlobTriggerFunction")]
        public static void Run([BlobTrigger("samples-workitems/{name}")] Stream myBlob, string name, ILogger log)
        {
            log.LogInformation($"C# Blob trigger function Processed blob\n Name:{name} \n Size: {myBlob.Length} Bytes");
        }
    }
}
 