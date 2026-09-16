using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP.BLL.Common
{
    public class EmailSender :IEmailSender
    {
        private readonly IConfiguration _configuration;

        public EmailSender(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public Task SendEmailAsync(string email, string subject, string Message)
        {
            //"smtp.gmail.com" عنوان السيرفر : 
            var client = new SmtpClient(_configuration["EmailSettings:Host"],int.Parse( _configuration["EmailSettings:Port"]))
            {
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(_configuration["EmailSettings:Email"], _configuration["EmailSettings:Password"])
            };
            //انشاء رسالة البريد الإلكتروني وإرسالها
            return client.SendMailAsync(
                new MailMessage(
                    from: _configuration["EmailSettings:Email"],
                    to: email,
                    subject: subject,
                    body: Message
                )
                { IsBodyHtml = true }
                 );
        }
    }
}
