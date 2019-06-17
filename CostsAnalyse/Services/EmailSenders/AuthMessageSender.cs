using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using MimeKit;
using MailKit;
using MailKit.Security;
using MailKit.Net.Smtp;

public class AuthMessageSender 
    {
    public async Task SendEmailAsync(string email, string subject, string message)
    {
        var emailMessage = new MimeMessage();

        emailMessage.From.Add(new MailboxAddress("Администрация сайта","ultwolf@gmail.com" ));
        emailMessage.To.Add(new MailboxAddress("", email));
        emailMessage.Subject = subject;
        emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
        {
            Text = message
        };

        using (var client = new MailKit.Net.Smtp.SmtpClient(new ProtocolLogger("smtp.log")))
        {
            await client.ConnectAsync("smtp.gmail.com", 465, SecureSocketOptions.SslOnConnect);

            await client.AuthenticateAsync(" ", " ");

            await client.SendAsync(emailMessage);

            await client.DisconnectAsync(true);
        }
    }
}