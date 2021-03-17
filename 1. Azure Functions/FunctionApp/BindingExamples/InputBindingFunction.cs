using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using FunctionApp.Models;

namespace FunctionApp.BindingExamples
{
    public static class InputBindingFunction
    {
        [FunctionName("InputBindingFunction")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            [CosmosDB(
                databaseName: "func-io-learn-db",
                collectionName: "Bookmarks",
                ConnectionStringSetting = "CosmosDBConnectionString",
                Id = "{Query.id}",
                PartitionKey = "{Query.id}")] Bookmark bookmark,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            if (bookmark is null)
            {
                return new NotFoundObjectResult("Not found.");
            }
            else
            {
                return new OkObjectResult(bookmark);
            }
        }
    }
}
