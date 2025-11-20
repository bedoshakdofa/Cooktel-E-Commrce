using Cooktel_E_commrece.Helper;
using Cooktel_E_commrece.Interfaces;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;


namespace Cooktel_E_commrece.Services
{
    public class EmailService:IEmailSender
    {
        private readonly MailSettings _settings;
        public EmailService(IOptions<MailSettings> settings)
        {
            _settings = settings.Value;
        }
        public async Task SendEmail(string emailTo,string body,string subject)
        {
            var email = new MimeMessage
            {
                Sender = MailboxAddress.Parse(_settings.EmailAddress),
                Subject = subject,
            };
            email.To.Add(MailboxAddress.Parse(emailTo));

            var builder = new BodyBuilder();


            builder.HtmlBody = body;
            email.Body=builder.ToMessageBody();
            email.From.Add(new MailboxAddress(_settings.DisplayName,_settings.EmailAddress));

            using var smtp = new SmtpClient();

            smtp.Connect(_settings.Host, _settings.Port);
            smtp.Authenticate(_settings.EmailAddress, _settings.Password);

            await smtp.SendAsync(email);

            smtp.Disconnect(true);

        }
    }
}
