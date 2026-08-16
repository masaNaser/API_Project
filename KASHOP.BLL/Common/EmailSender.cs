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
        public Task SendEmailAsync(string email, string subject, string Message)
        {
            //"smtp.gmail.com" عنوان السيرفر : 
            var client = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential("masanaser2003@gmail.com", "hgzi iizk uvdf ldro")
            };
            //انشاء رسالة البريد الإلكتروني وإرسالها
            return client.SendMailAsync(
                new MailMessage(
                    from: "masanaser2003@gmail.com",
                    to: email,
                    subject: subject,
                    body: Message
                )
                { IsBodyHtml = true }
                 );
        }
    }
}
