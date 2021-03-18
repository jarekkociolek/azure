using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using FunctionApp.Models;
using Newtonsoft.Json;
using System.IO;

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
                [Queue("bookmarks-queue", Connection = "StorageConnectionAppSetting")] out string newMessage,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string requestBody = new StreamReader(req.Body).ReadToEnd();
            dynamic data = JsonConvert.DeserializeObject(requestBody);

            newBookmark = null;
            newMessage = null;

            if (bookmark is null)
            {
                newBookmark = new { data.id, data.url };
                newMessage = JsonConvert.SerializeObject(newBookmark);
                return new OkObjectResult($"Created db object: {newBookmark} and queue message: {newMessage}");
            }
            else
            {
                return new OkObjectResult(bookmark);
            }
        }
    }
}
