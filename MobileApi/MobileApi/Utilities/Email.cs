using System.Configuration;
using System.Net;
using System.Net.Mail;

namespace MobileApi.Utilities
{
    public class Email
    {
        // Note: To send email you need to add actual email id and credential so that it will work as expected  
        static string smtpAddress = "smtp.gmail.com";
        static int portNumber = 587;
        static bool enableSSL = true;
        static string emailFromAddress = "agarwala.uw@gmail.com"; //Sender Email Address  
        static string password = ConfigurationManager.AppSettings["password"]; //"Abc@123$%^"; //Sender Password  
        static string subject = "Hello";

        public static void SendEMail(string recipient, string subject, string body)
        {
            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress(emailFromAddress);
                mail.To.Add(recipient);
                mail.Subject = subject;
                mail.Body = body;
                mail.IsBodyHtml = true;
                //mail.Attachments.Add(new Attachment("D:\\TestFile.txt"));//--Uncomment this to send any attachment  
                using (SmtpClient smtp = new SmtpClient(smtpAddress, portNumber))
                {
                    smtp.Credentials = new NetworkCredential(emailFromAddress, password);
                    smtp.EnableSsl = enableSSL;
                    smtp.Send(mail);
                }
            }
        }
    }
}