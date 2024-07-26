using System.Net.Mail;
using System.Net;

namespace Phat_Blogger_Website.Helpers
{
    public class SendEmail : ISendEmail
    {
        public Task SendEmailAsync(string email, string subject, string message)
        {
            var client = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential("testinggmail2212@gmail.com", "testing@2212")
            };

            return client.SendMailAsync(
                new MailMessage(from: "testinggmail2212@gmail.com",
                                to: email,
                                subject,
                                message
                                ));
        }
    }
}
