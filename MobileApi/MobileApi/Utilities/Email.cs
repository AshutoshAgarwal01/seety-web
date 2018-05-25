using MobileApi.Models;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web.UI.WebControls;

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
        static string subject = "Order received!";

        public static async Task SendEmailAsync(Order order)
        {
            var apiKey = System.Environment.GetEnvironmentVariable("SENDGRIDAPI_KEY");
            var client = new SendGridClient(apiKey);
            var msg = new SendGridMessage()
            {
                From = new EmailAddress(emailFromAddress, "Seety Team"),
                Subject = subject,
                //PlainTextContent = "Hello, Email!",
                //HtmlContent = "<strong>Hello, Email!</strong>"
                HtmlContent = GetEmailBody(order)
            };
            msg.AddTo(new EmailAddress(order.Customer.Email, "Test User"));
            var response = await client.SendEmailAsync(msg);
        }

        private static string GetEmailBody(Order order)
        {
            var orderCreatedOn = DateTime.Now.ToString("dddd, dd MMMM yyyy h:mm tt");
            string fullAddress = GetAddress(order);
            string selectedOptionBlock = GetSelectedOptionBlock(order); //Assuming that there is only one prderline per order.
            string moreProjInfo = order.OrderLines[0].MoreInformation; //Assuming that there is only one prderline per order.
            string projSchedule = order.OrderLines[0].ProjectTime.ToString("dddd, dd MMMM yyyy h:mm tt"); //Assuming that there is only one prderline per order.

            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"EmailHtml\BasicMor.html");
            string emailHtml = File.ReadAllText(path);

            emailHtml = emailHtml.Replace("{ordernumber}", order.OrderId.ToString());
            emailHtml = emailHtml.Replace("{orderCreatedOn}", orderCreatedOn);
            emailHtml = emailHtml.Replace("{fullAddress}", fullAddress);
            emailHtml = emailHtml.Replace("{selectedOptionBlock}", selectedOptionBlock);
            emailHtml = emailHtml.Replace("{moreProjInfo}", moreProjInfo);
            emailHtml = emailHtml.Replace("{projSchedule}", projSchedule);

            return emailHtml;
        }

        public static void SendEMailSmtp(Order order)
        {
            Trace.TraceError("Email start");

            MailDefinition md = new MailDefinition();
            md.From = emailFromAddress;
            md.IsBodyHtml = true;
            md.Subject = subject;
            var recipient = order.Customer.Email;
            var orderCreatedOn = DateTime.Now.ToString("dddd, dd MMMM yyyy h:mm tt");
            string fullAddress = GetAddress(order);
            string selectedOptionBlock = GetSelectedOptionBlock(order); //Assuming that there is only one prderline per order.
            string moreProjInfo = order.OrderLines[0].MoreInformation; //Assuming that there is only one prderline per order.
            string projSchedule = order.OrderLines[0].ProjectTime.ToString("dddd, dd MMMM yyyy h:mm tt"); //Assuming that there is only one prderline per order.

            /*ListDictionary replacements = new ListDictionary();
            replacements.Add("{ordernumber}", order.OrderId.ToString());
            replacements.Add("{orderCreatedOn}", orderCreatedOn);
            replacements.Add("{fullAddress}", fullAddress);
            replacements.Add("{selectedOptionBlock}", selectedOptionBlock);
            replacements.Add("{moreProjInfo}", moreProjInfo);
            replacements.Add("{projSchedule}", projSchedule);

            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"EmailHtml\BasicMor.html");
            string emailHtml = File.ReadAllText(path);*/

            ListDictionary replacements = new ListDictionary();
            replacements.Add("name", "Ashutosh");
            string emailHtml = @"<p>This is a test {name}</p>";

            MailMessage msg = md.CreateMailMessage(recipient, replacements, emailHtml, new System.Web.UI.Control());

            Trace.TraceError("Email: before smtp client");
            using (SmtpClient smtp = new SmtpClient(smtpAddress, portNumber))
            {
                smtp.Credentials = new NetworkCredential(emailFromAddress, password);
                smtp.EnableSsl = enableSSL;
                smtp.Send(msg);
            }
            Trace.TraceError("Email end");

        }

        private static string GetSelectedOptionBlock(Order order)
        {
            string template = @"<tr>
                                <td class=""cell - content product - info"" style=""font - family: Verdana, Arial; font - weight: normal; border - collapse: collapse; vertical - align: top; padding: 10px 15px; margin: 0; border - top: 1px solid #ebebeb; text-align: left;"">
                                    <p class=""product-name"" style=""font-family: Verdana, Arial; font-weight: bold; margin: 0 0 5px 0; color: #636363; font-style: normal; text-rendering: optimizeLegibility; line-height: 1.4; font-size: 12px; float: left; width: 100%; display: block;"">{0}</p>
                                    <p class=""sku"" style=""font-family: Verdana, Arial; font-weight: normal; margin: 0 0 5px; float: left; width: 100%; display: block; font-size: 12px;"">{1}</p>
                                </td>
                            </tr>";
            string block = string.Empty;

            //For now, assuming that there will be only one order line per order.
            var currNode = order.OrderLines[0].ServiceInfo;
            block = string.Format(template, "How may we help you?", currNode.Name);

            while(currNode != null && currNode.ChildrenNodes != null && currNode.ChildrenNodes.Count > 0)
            {
                var option = currNode.Description;
                string response = string.Empty;
                var i = 0;
                foreach(var c in currNode.ChildrenNodes)
                {
                    if (i != 0)
                        response += ", ";
                    response += c.Name;
                    i++;
                }
                block += string.Format(template, option, response);
                currNode = currNode.ChildrenNodes[0];
            }
            return block;
        }

        private static string GetAddress(Order order)
        {
            var newLine = @"<br />";
            var fullAddress = order.Customer.FirstName + " " + order.Customer.LastName;
            fullAddress += string.IsNullOrEmpty(fullAddress) ? string.Empty : newLine + order.Location.Street;

            var cityStateAndPin  = order.Location.City ?? string.Empty;
            if (!string.IsNullOrEmpty(order.Location.State))
            {
                if (!string.IsNullOrEmpty(cityStateAndPin))
                {
                    cityStateAndPin += ", ";
                }
                cityStateAndPin += order.Location.State;
            }

            if (!string.IsNullOrEmpty(order.Location.Pin))
            {
                if (!string.IsNullOrEmpty(cityStateAndPin))
                {
                    cityStateAndPin += ", ";
                }
                cityStateAndPin += order.Location.Pin;
            }

            if (!string.IsNullOrEmpty(cityStateAndPin))
            {
                if (!string.IsNullOrEmpty(fullAddress))
                {
                    fullAddress += newLine;
                }
                fullAddress += cityStateAndPin;
            }

            if (!string.IsNullOrEmpty(order.Customer.PhoneNumber))
            {
                if (!string.IsNullOrEmpty(fullAddress))
                {
                    fullAddress += newLine;
                }
                fullAddress += "T: " + order.Customer.PhoneNumber;
            }

            return fullAddress;
        }

    }
}