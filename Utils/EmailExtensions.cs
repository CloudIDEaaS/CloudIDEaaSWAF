using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Utils
{
    public static class EmailExtensions
    {
        public static void SendEmail(string from, string to, string subject, string body)
        {
            var message = new MailMessage();
            var client = new SmtpClient();

            message.From = new MailAddress(from);
            message.To.Add(new MailAddress(to));
            message.Subject = subject;
            message.Body = body;

            try
            {
                // client.Send(message);
            }
            catch
            {
            }
        }
    }
}
