using Demo.DAL.Models;
using Demo.PL.Settings;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using System.Net;
//using System.Net.Mail;

namespace Demo.PL.Helpers
{
	public class EmailSettings:IMailService
	{
        private MailSettings _options;

        /*public static void SendEmail(Email email)
{
var Client = new SmtpClient("...", 123);
Client.EnableSsl = true;
Client.Credentials = new NetworkCredential("...", "...");
Client.Send("...",email.To,email.Subject, email.Body);
}*/
        public EmailSettings(IOptions<MailSettings> options)
        {
            _options = options.Value;
        }
        public void SendEmail(Email email)
        {
            var mail = new MimeMessage
            {
                Sender = MailboxAddress.Parse(_options.Email),
                Subject = email.Subject,
            };
            mail.To.Add(MailboxAddress.Parse(email.To));
            mail.From.Add(new MailboxAddress(_options.DisplayName, _options.Email));
            
            var builder = new BodyBuilder();
            builder.TextBody = email.Body;

            mail.Body = builder.ToMessageBody();

            using var smtp = new SmtpClient();

            smtp.Connect(_options.Host, _options.Port,MailKit.Security.SecureSocketOptions.StartTls);
            smtp.Authenticate(_options.Email, _options.Password);

            smtp.Send(mail);
            smtp.Disconnect(true);
        }
    }
}
