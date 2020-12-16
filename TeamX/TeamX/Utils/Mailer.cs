using System;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace TeamX.Utils
{
    public static class Mailer
    {
        public static void SendPasswordEmail(string receiver_address ,string receiver_name,  string pwd)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("TeamX", ""));
            message.To.Add(new MailboxAddress(receiver_name, receiver_address));
            message.Subject = "TeamX credentials";

            message.Body = new TextPart("plain")
            {
                Text = @"Hi " + receiver_name + ",\n" +
                	"Here are your credentials to enter the app: \n" +
                	"Name : " + receiver_name +
                	"\nPassword : " + pwd +
                	"\n\nHave fun! " +
                	"\n-- TeamX "
            };

            using (var client = new SmtpClient())
            {
                // For demo-purposes, accept all SSL certificates (in case the server supports STARTTLS)
                client.ServerCertificateValidationCallback = (s, c, h, e) => true;

                client.Connect("smtp.gmail.com", 465, SecureSocketOptions.SslOnConnect);

                // Note: only needed if the SMTP server requires authentication
                client.Authenticate("", "");

                client.Send(message);
                client.Disconnect(true);
            }
        }
    }
}
