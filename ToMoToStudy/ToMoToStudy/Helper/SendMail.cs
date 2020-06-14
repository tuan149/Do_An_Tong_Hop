using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace ToMoToStudy.Helper
{
    public static class SendMail
    {
        public static string EmailUser;
        public static string EmailPassword;
        public static string EmailName;
        public static string EmailSMTP = "smtp.gmail.com";
        public static string EmailSMTPPort = "587";

        public static void Send(string toEmail, string subject, string body)
        {
            if (String.IsNullOrEmpty(EmailUser) || String.IsNullOrEmpty(EmailPassword) || String.IsNullOrEmpty(EmailSMTP) || String.IsNullOrEmpty(EmailSMTPPort)) throw new Exception("Kỹ thuật viên chưa truyền dữ liệu email");

            var smtp = new SmtpClient
            {
                Host = EmailSMTP,
                Port = int.Parse(EmailSMTPPort),
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(EmailUser, EmailPassword)
            };

            // add from,to mailaddresses
            MailAddress from = new MailAddress(EmailUser, EmailName);
            MailAddress to = new MailAddress(toEmail);
            MailMessage myMail = new System.Net.Mail.MailMessage(from, to);

            // set subject and encoding
            myMail.Subject = subject;
            myMail.SubjectEncoding = System.Text.Encoding.UTF8;
            // set body-message and encoding
            myMail.Body = body;
            myMail.BodyEncoding = System.Text.Encoding.UTF8;

            // text or html
            myMail.IsBodyHtml = true;

            smtp.Send(myMail);
        }
    }
}