using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace BAL.Services
{
    public class EmailServices
    {
        private readonly IConfiguration _config;

        public EmailServices(IConfiguration config)
        {
            _config = config;
        }
        public async Task SendEmailAsync(string toEmail,string subject,string body)       
        {
            try
            {
                var fromEmail =
                    _config["EmailSettings:FromEmail"];

                var fromPassword =
                    _config["EmailSettings:AppPassword"];

                using var smtpClient = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587,
                    Credentials = new NetworkCredential(
                        fromEmail,
                        fromPassword),

                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false
                };

                using var mailMessage = new MailMessage
                {
                    From = new MailAddress(fromEmail!),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                };

                mailMessage.To.Add(toEmail);

                await smtpClient.SendMailAsync(mailMessage);
            }
            catch (SmtpException ex)
            {
                throw new Exception("SMTP Error: " + ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Email Service Error: " + ex.Message);
            }
        }
    }
}
