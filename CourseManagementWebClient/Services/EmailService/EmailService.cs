using CourseManagementWebClient.Services.EmailService;
using CourseManagementWebClientWebClient.Models;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Gmail.v1;
using Google.Apis.Gmail.v1.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Text;

namespace CourseManagementWebClientWebClient.Services.EmailService
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _emailSettings;
        private readonly GoogleCredentials _googleCredentials;

        public EmailService(IOptions<EmailSettings> emailSettings, IOptions<GoogleCredentials> googleCredentials)
        {
            _emailSettings = emailSettings.Value;
            _googleCredentials = googleCredentials.Value;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            var email = new MimeMessage();
            email.From.Add(new MailboxAddress(_emailSettings.SenderName, _emailSettings.SenderEmail));
            email.To.Add(MailboxAddress.Parse(toEmail));
            email.Subject = subject;

            var builder = new BodyBuilder();
            builder.HtmlBody = body;
            email.Body = builder.ToMessageBody();

            using (var smtpClient = new SmtpClient())
            {
                var credential = await GetCredential();
                smtpClient.Connect(_emailSettings.SmtpServer, _emailSettings.SmtpPort);
                smtpClient.Authenticate(_emailSettings.SmtpUsername, credential.Token.AccessToken);

                await smtpClient.DisconnectAsync(true);
            }
        }

        private async Task<UserCredential> GetCredential()
        {
            UserCredential credential;

            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes($"{{\"web\":{{\"client_id\":\"{_googleCredentials.ClientId}\",\"client_secret\":\"{_googleCredentials.ClientSecret}\"}}}}")))
            {
                credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    new[] { GmailService.Scope.GmailSend },
                    "user",
                    CancellationToken.None
                );
            }

            return credential;
        }
    }
}

