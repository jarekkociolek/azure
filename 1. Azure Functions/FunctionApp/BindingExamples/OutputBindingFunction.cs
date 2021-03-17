using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using FunctionApp.Models;
using Newtonsoft.Json;

namespace FunctionApp.BindingExamples
{
    public static class OutputBindingFunction
    {
        [FunctionName("OutputBindingFunction")]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            [CosmosDB(
                databaseName: "func-io-learn-db",
                collectionName: "Bookmarks",
                ConnectionStringSetting = "CosmosDBConnectionString",
                Id = "{Query.id}",
                PartitionKey = "{Query.id}")] Bookmark bookmark,
                [CosmosDB(
                databaseName: "func-io-learn-db",
                collectionName: "Bookmarks",
                ConnectionStringSetting = "CosmosDBConnectionString")] out dynamic newBookmark,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            newBookmark = null;
            if (bookmark is null)
            {
                string id = req.Query["id"];
                string url = req.Query["url"];

                newBookmark = new { id, url };
                return new NotFoundObjectResult($"Created: {newBookmark}");
            }
            else
            {
                return new OkObjectResult(bookmark);
            }
        }
    }
}
