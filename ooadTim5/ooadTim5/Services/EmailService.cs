using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace ooadTim5.Services
{
    public class EmailService
    {
        private readonly IConfiguration _config;
        private readonly HttpClient _httpClient;

        public EmailService(IConfiguration config, HttpClient httpClient)
        {
            _config = config;
            _httpClient = httpClient;
        }

        public async Task PosaljiPotvrdu(string email, string link)
        {
            var apiKey = _config["BrevoSettings:ApiKey"];

            var payload = new
            {
                sender = new { name = "LitHub", email = "lithubssa@gmail.com" },
                to = new[] { new { email = email } },
                subject = "Potvrda registracije - LitHub",
                htmlContent = $@"
                    <h2>Dobrodošli u LitHub!</h2>
                    <p>Kliknite na link ispod da potvrdite registraciju:</p>
                    <a href='{link}' style='background:#c8621a; color:white; 
                       padding:10px 25px; border-radius:25px; text-decoration:none;
                       font-weight:700;'>Potvrdi registraciju</a>"
            };

            var request = new HttpRequestMessage(HttpMethod.Post, "https://api.brevo.com/v3/smtp/email");
            request.Headers.Add("api-key", apiKey);
            request.Content = new StringContent(
                JsonSerializer.Serialize(payload),
                Encoding.UTF8,
                "application/json"
            );

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
        }
    }
}