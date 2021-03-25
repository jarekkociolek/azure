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
            string txt = null;
            using (var reader = new StreamReader(req.Body))
            {
                txt = await reader.ReadToEndAsync();
            }
            log.LogError(txt);

            if (!(await IsAuthorized(req, log)))
            {
                return new UnauthorizedResult();
            }

            log.LogInformation("C# HTTP trigger function processed a request.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);

            if (data?.repository?.name != null)
            {
                return new OkObjectResult($"Repository is: {data.repository.name}, Event Type is: {req.Headers["x-github-event"]}");
            }

            return new BadRequestResult();
        }

        private static async Task<bool> IsAuthorized(HttpRequest request, ILogger log)
        {
            string githubSignature = request.Headers["x-hub-signature"];
            var inSignature = githubSignature.Substring("sha1=".Length);

            if (githubSignature is null)
            {
                return false;
            }
            else
            {
                var signature = await HashHMAC(request.Body);
                var stringSignature = ToHexString(signature);
                log.LogInformation(stringSignature);
                return String.Equals(inSignature, stringSignature);
            }
        }

        private static async Task<byte[]> HashHMAC(Stream body)
        {
            string txt = null;
            using (var reader = new StreamReader(body))
            {
                txt = await reader.ReadToEndAsync();
            }
            var keyBytes = Encoding.ASCII.GetBytes("GesphaCjVmcVTtDmd52HaVeARx6yVg9gjJ8ypqZyhOt5DUx1q/ufVQ==");

            var hash = new HMACSHA1(keyBytes);
            return hash.ComputeHash(Encoding.ASCII.GetBytes(txt));
        }

        public static string ToHexString(byte[] bytes)
        {
            var builder = new StringBuilder(bytes.Length * 2);
            foreach (byte b in bytes)
            {
                builder.AppendFormat("{0:x2}", b);
            }

            return builder.ToString();
        }
    }
}
