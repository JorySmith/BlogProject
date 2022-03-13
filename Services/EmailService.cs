using BlogProject.ViewModels;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogProject.Services
{
  public class EmailService : IBlogEmailSender
  {
    // Store instance of MailSettings, add to constructor
    private readonly MailSettings _mailSettings;

    // Constructor, update with IOptions
    public EmailService(IOptions<MailSettings> mailSettings)
    {
      _mailSettings = mailSettings.Value; // Grab the .Value
    }

    // Contact me email 
    public async Task SendContactEmailAsync(string emailFrom, string name, string subject, string htmlMessage)
    {
      var email = new MimeMessage();
      email.Sender = MailboxAddress.Parse(_mailSettings.Mail);
      email.To.Add(MailboxAddress.Parse(_mailSettings.Mail));
      email.Subject = subject;

      var builder = new BodyBuilder();
      builder.HtmlBody = $"<b>{name}</b> has sent you an email and can be reached at: <b>{emailFrom}</b><br/><br/>{htmlMessage}";

      email.Body = builder.ToMessageBody();

      using var smtp = new SmtpClient();
      smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
      smtp.Authenticate(_mailSettings.Mail, _mailSettings.Password);

      await smtp.SendAsync(email);

      smtp.Disconnect(true);
    }

    // User registration email 
    public async Task SendEmailAsync(string emailTo, string subject, string htmlMessage)
    {
      // Instantiate a new MimeMessage() to store email response properties
      var email = new MimeMessage();
      email.Sender = MailboxAddress.Parse(_mailSettings.Mail); // my email
      email.To.Add(MailboxAddress.Parse(emailTo)); // their email
      email.Subject = subject;

      // Use BodyBuilder() to configure email body
      var builder = new BodyBuilder()
      {
        HtmlBody = htmlMessage
      };
      email.Body = builder.ToMessageBody();

      // Use SmtpClient() from MailKit NuGet package, to configure smtp
      using var smtp = new SmtpClient();
      smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
      smtp.Authenticate(_mailSettings.Mail, _mailSettings.Password);

      // Send email async
      await smtp.SendAsync(email);

      // Disconnect smtp
      smtp.Disconnect(true);

      // Can add attachments here if needed
    }
  }
}
