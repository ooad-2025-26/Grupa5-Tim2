using System.Net;
using System.Net.Mail;

namespace ooadTim5.Services
{
    public class EmailService
    {
        private readonly IConfiguration _config;

        public EmailService(IConfiguration config)
        {
            _config = config;
        }

        public async Task PosaljiPotvrdu(string email, string link)
        {
            var from = _config["EmailSettings:Email"];
            var password = _config["EmailSettings:Password"];

            var smtp = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential(from, password),
                EnableSsl = true
            };

            var poruka = new MailMessage(from!, email)
            {
                Subject = "Potvrda registracije - LitHub",
                Body = $@"
                    <h2>Dobrodošli u LitHub!</h2>
                    <p>Kliknite na link ispod da potvrdite registraciju:</p>
                    <a href='{link}' style='background:#c8621a; color:white; 
                       padding:10px 25px; border-radius:25px; text-decoration:none;
                       font-weight:700;'>Potvrdi registraciju</a>",
                IsBodyHtml = true
            };

            await smtp.SendMailAsync(poruka);
        }
    }
}