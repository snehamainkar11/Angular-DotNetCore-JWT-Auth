namespace AuthECAPI.FunctionTriggers
{
    public interface IFunctionInvoker
    {
        Task<bool> TriggerWelcomeEmail(string email, string name);
    }

    public class FunctionInvoker : IFunctionInvoker
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;

        public FunctionInvoker(HttpClient httpClient, IConfiguration config)
        {
            _httpClient = httpClient;
            _config = config;
        }

        public async Task<bool> TriggerWelcomeEmail(string email, string name)
        {
            try
            {
                var functionUrl = _config["AzureFunctions:SendWelcomeEmailUrl"];
                var functionKey = _config["AzureFunctions:SendWelcomeEmailKey"];

                var request = new HttpRequestMessage(HttpMethod.Post, functionUrl);
                request.Headers.Add("x-functions-key", functionKey);
                request.Content = JsonContent.Create(new { Email = email, Name = name });

                var response = await _httpClient.SendAsync(request);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {

                return false;
            }
        }
    }
}
