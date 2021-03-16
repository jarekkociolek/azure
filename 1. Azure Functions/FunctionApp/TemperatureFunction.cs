using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace FunctionApp
{
    public static class Function
    {
        private const string OkStatus = "OK";
        private const string CautionStatus = "CAUTION";
        private const string DangerStatus = "DANGER";

        [FunctionName("TemperatureFunction")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {

            log.LogInformation("C# HTTP trigger function processed a request.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);

            if (data.readings != null)
            {
                foreach (var reading in data.readings)
                {
                    if (reading.temperature <= 25)
                    {
                        reading.status = OkStatus;
                    }
                    else if (reading.temperature <= 50)
                    {
                        reading.status = CautionStatus;
                    }
                    else
                    {
                        reading.status = DangerStatus;
                    }
                }
                return new OkObjectResult(data.readings);
            }

            string responseMessage = "Missing readings in body";
            return new OkObjectResult(responseMessage);
        }
    }
}
