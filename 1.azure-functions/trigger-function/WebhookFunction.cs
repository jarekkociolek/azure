using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Text;
using System.Text.Unicode;

namespace trigger_function
{
    public static class WebhookFunction
    {
        [FunctionName("WebhookFunction")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            if (!IsAuthorized(req, log))
            {
                return new UnauthorizedResult();
            }

            log.LogInformation("C# HTTP trigger function processed a request.");

            string name = req.Query["name"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            name = name ?? data?.name;

            if (data?.repository?.name != null)
            {
                return new OkObjectResult($"Repository is: {data.repository.name}, Event Type is: {req.Headers["x-github-event"]}");
            }

            return new BadRequestResult();
        }

        private static bool IsAuthorized(HttpRequest request, ILogger log)
        {
            string githubSignature = request.Headers["x-hub-signature"];

            if (githubSignature is null)
            {
                return false;
            }
            else
            {
                var signature = HashHMAC();
                log.LogInformation(Encoding.UTF8.GetString(signature));
                return String.Equals(githubSignature, Encoding.UTF8.GetString(signature));
            }
        }

        private static byte[] HashHMAC()
        {
            var keyBytes = Encoding.UTF8.GetBytes("sha1");

            var hash = new HMACSHA256(keyBytes);
            return hash.ComputeHash(Encoding.UTF8.GetBytes("GesphaCjVmcVTtDmd52HaVeARx6yVg9gjJ8ypqZyhOt5DUx1q/ufVQ=="));
        }
    }
}
