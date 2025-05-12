using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace Attendance.Services
{
    public class WhatsAppService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public WhatsAppService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task SendMessageAsync(string mobileNumber, string message)
        {
            var instanceId = _configuration["WhatsAppSettings:InstanceId"];
            var token = _configuration["WhatsAppSettings:Token"];

            var url = $"https://api.ultramsg.com/{instanceId}/messages/chat?token={token}";

            var payload = new
            {
                to = mobileNumber,
                body = message
            };

            var request = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json")
            };

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            try
            {
                var response = await _httpClient.SendAsync(request);
                var responseBody = await response.Content.ReadAsStringAsync();

                // Log status code and response content
                Console.WriteLine($"[WhatsAppService] Status Code: {response.StatusCode}");
                Console.WriteLine($"[WhatsAppService] Response: {responseBody}");

                // Optional: throw if not successful
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[WhatsAppService] Error sending message: {ex.Message}");
                throw;
            }
        }

    }
}
