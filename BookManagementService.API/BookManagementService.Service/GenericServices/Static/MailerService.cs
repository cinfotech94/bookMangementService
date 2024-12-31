using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Runtime.CompilerServices;
using BookManagementService.Service.GenericServices.Interface;
using BookManagementService.Domain.DTO.Common;

namespace BookManagementService.Service.GenericServices.Static
{
    public class MailerService
    {
        private readonly IConfiguration _configuration;
        private readonly ILoggingService _loggingService;

        public MailerService(ILoggingService loggingService, IConfiguration configuration)
        {
            _configuration = configuration;
            _loggingService = loggingService;
        }

        public async Task<bool> SendEmail(
            string to,
            string subject,
            string body,
            [CallerMemberName] string caller = "")
        {
            if (string.IsNullOrWhiteSpace(to))
            {
                await _loggingService.LogError("To address is missing.", caller, new ArgumentNullException(nameof(to)));
                throw new ArgumentNullException(nameof(to), "To address is required.");
            }

            try
            {
                // Retrieve SMTP settings from configuration
                string smtpHost = _configuration["MailSettings:MailHost"];
                int smtpPort = int.Parse(_configuration["MailSettings:MailPort"]);
                string senderEmail = _configuration["MailSettings:SenderEmail"];
                string senderPassword = _configuration["MailSettings:SenderPassword"];

                // Create the mail message
                var mailMessage = new MailMessage
                {
                    From = new MailAddress(senderEmail),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                };
                mailMessage.To.Add(to);

                // Set up the SMTP client
                using var smtpClient = new SmtpClient(smtpHost, smtpPort)
                {
                    Credentials = new NetworkCredential(senderEmail, senderPassword),
                    EnableSsl = true
                };

                // Send the email
                await smtpClient.SendMailAsync(mailMessage);

                await _loggingService.LogInformation($"Email sent successfully to {to}.", caller);
                return true;
            }
            catch (Exception ex)
            {
                await _loggingService.LogFatal("An error occurred while sending the email.", caller, ex);
                throw; // Re-throw the exception after logging it
            }
        }
    }
}
