using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Text;
using System.Runtime.InteropServices;
using System.Net.Mail;
using System.Threading.Tasks;

namespace AprosysAccounting.Appcode
{
    public class Email
    {

        #region Email
        public class EmailConfig
        {
            public int port { get; set; }
            public string host { get; set; }
            public bool isEnableSsl { get; set; }
            public string email { get; set; }
            public string SMTPUserName { get; set; }
            public string password { get; set; }

            public string FromEmail { get; set; }
            public string FromName { get; set; }
        }
        public static EmailConfig getEmailConfig()
        {
            return new EmailConfig
            {
                port = Convert.ToInt32(ConfigurationManager.AppSettings["SenderSMTPServerPort"]),
                host = ConfigurationManager.AppSettings["SenderSMTPServer"],
                email = ConfigurationManager.AppSettings["SenderEmailAddress"],
                password = ConfigurationManager.AppSettings["SenderEmailPassword"],
                SMTPUserName = ConfigurationManager.AppSettings["SenderEmailAddress"],
                isEnableSsl = Convert.ToBoolean(ConfigurationManager.AppSettings["isEnableSsl"]),
                FromEmail = ConfigurationManager.AppSettings["FromEmailAddress"],
                FromName = ConfigurationManager.AppSettings["FromEmailName"],
            };
        }
        public static string PopulateEmailBody(string Message)
        {
            StringBuilder message = new StringBuilder(String.Empty);
            message.Append("Dear Adminstrator <br/><br/><br/>");
            message.Append("Inseration/Updating through Exception. Exception Detail Below<br/><br/>");
            message.Append("<b>TimeStamp:</b> " + DateTime.Now.ToString() + "<br/><br/>");
            message.Append("<b>Message:</b> " + Message + "<br/><br/>");


            message.Append("<br/>Regard's");
            return message.ToString();
        }

        public static void sendEmail(string _message, string Subject, [Optional]string toEmailAddress, [Optional] bool MessageIsHTML, [Optional] SortedList<string, string> attachmentPath, [Optional] string FromAddress)
        {
            try
            {
                string message = String.IsNullOrEmpty(toEmailAddress) && !MessageIsHTML ? PopulateEmailBody(_message) : _message;
                string toEmail = String.IsNullOrEmpty(toEmailAddress) ? ConfigurationManager.AppSettings["ToEmailAddress"] : toEmailAddress;
                var config = getEmailConfig();
                if (ConfigurationManager.AppSettings["EmailSandBoxEnabled"] == "true" && toEmail != ConfigurationManager.AppSettings["ToEmailAddress"])
                {
                    Subject = Subject + "Email Changed from " + toEmail;
                    toEmail = ConfigurationManager.AppSettings["TestingToEmail"];
                }
                SmtpClient client = new SmtpClient();
                client.Port = config.port;
                client.Host = config.host;
                client.EnableSsl = config.isEnableSsl;
                client.Timeout = 1800000;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Credentials = new System.Net.NetworkCredential(config.SMTPUserName, config.password);

                using (MailMessage mm = new MailMessage(config.email, toEmail, Subject, message))
                {
                    mm.From = new MailAddress(config.FromEmail, config.FromName);
                    if (attachmentPath != null && attachmentPath.Count > 0)
                    {
                        foreach (var item in attachmentPath)
                        {
                            var stream = System.IO.File.OpenRead(item.Key);
                            mm.Attachments.Add(new Attachment(stream, item.Value));

                        }

                    }
                    mm.Bcc.Add(ConfigurationManager.AppSettings["ToEmailAddress"]);

                    mm.BodyEncoding = UTF8Encoding.UTF8;
                    mm.IsBodyHtml = true;
                    mm.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
                    if (FromAddress != null)
                    {
                        mm.From = new MailAddress(FromAddress);
                    }
                    Logger.Write("SendingEmail", "TO:" + toEmail + " Subject " + Subject, "", Logger.LogType.InformationLog);
                    client.Send(mm);
                }
            }
            catch (Exception ex)
            {
                Logger.Write("Error in sending email " + ex.ToString(), "", "", Logger.LogType.ErrorLog);
                throw;
            }

        }

        public static void sendEmailAsync(string _message, string Subject, [Optional]string toEmail)
        {
            var task = new Task(() =>
            {

                try
                {
                    sendEmail(_message, Subject, toEmail);
                }
                catch (Exception ex)
                {
                    Logger.Write("Error in sending email " + ex.ToString(), "", "", Logger.LogType.ErrorLog);
                }
            });
            task.Start();
            task.Wait(10000);
        }
        #endregion

    }
}