using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net;

namespace AuthDemoAzure
{
    public class Function1
    {
        private readonly ILogger<Function1> _logger;

        public Function1(ILogger<Function1> logger)
        {
            _logger = logger;
        }

        [Function("sendemail")]
        public async Task<HttpResponseData> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData req)
        {
            var body = await new StreamReader(req.Body).ReadToEndAsync();
            var data = JsonConvert.DeserializeObject<dynamic>(body);

            string email = data?.Email;
            string name = data?.Name;

            _logger.LogInformation($"Sending welcome email to {email}");

            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteStringAsync($"Email sent to {name}");
            return response;
        }
    }
}