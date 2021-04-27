using FacilityManagement.Services.Core.Interfaces;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Threading.Tasks;

namespace FacilityManagement.Services.Core.Implementation
{
    public class MailService : IMailService
    {
        private readonly Models.AppSettingModels.MailSettings _mailSettings;

        public MailService(IOptions<Models.AppSettingModels.MailSettings> mailSettings)
        {
            _mailSettings = mailSettings.Value;
        }

        public async Task<bool> SendEmail(string recepient, string message, string msgSubject)
        {
            try
            {
                var senderEmail = _mailSettings.SenderEmail;
                var apiKey = _mailSettings.SendGrid_API_KEY;
                var client = new SendGridClient(apiKey);
                var from = new EmailAddress(senderEmail, "Decagon");
                var subject = msgSubject;
                var to = new EmailAddress(recepient);
                var plainTextContent = message;
                var htmlContent = message;
                var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
                var response = await client.SendEmailAsync(msg).ConfigureAwait(false);
                if (response.StatusCode.ToString().ToLower() == "accepted")
                {
                    return true;
                }
                else
                {
                    throw new ArgumentException("Mail was unable to send");
                }
            }
            catch (Exception) { }
            return false;
        }
    }
}