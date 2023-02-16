using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace DatingApp.Services.Helpers
{
    public class EmailHelper
    {
        private readonly IOptions<SmtpSettings> _smtpSettings;

        public EmailHelper(IOptions<SmtpSettings> smtpSettings)
        {
            _smtpSettings = smtpSettings;
        }

        public async Task SendMailAsync(string to, string subject, string text)
        {
            var mailMessage = new MailMessage(_smtpSettings.Value.SenderEmail, to, subject, text);

            using (var emailClient = new SmtpClient(_smtpSettings.Value.Host, _smtpSettings.Value.Port))
            {
                emailClient.Credentials = new NetworkCredential(
                    _smtpSettings.Value.User,
                    _smtpSettings.Value.Password);

                await emailClient.SendMailAsync(mailMessage);
            }
        }
    }

    public class SmtpSettings
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
        public string SenderEmail { get; set; }
    }
}
