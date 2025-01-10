using System;
using System.Configuration;
using System.Net.Mail;

namespace NguyenPhanHuy_2122110062.Common
{
    public class Common
    {
        private static string password = ConfigurationManager.AppSettings["Password"];
        private static string email = ConfigurationManager.AppSettings["Email"];

        public static bool SendMail(string name, string subject, string body, string toMail)
        {
            bool result = false;
            try
            {
                MailMessage message = new MailMessage();
                var smtp = new SmtpClient();
                {
                    smtp.Host = "smtp.gmail.com";
                    smtp.Port = 587;
                    smtp.EnableSsl = true;
                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;

                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new System.Net.NetworkCredential()
                    {
                        UserName = email,
                        Password = password
                    };
                }

                MailAddress fromAddress = new MailAddress(email, name);
                message.From = fromAddress;
                message.To.Add(toMail);
                message.Subject = subject;
                message.IsBodyHtml = true;
                message.Body = body;

                smtp.Send(message);
                result = true;
            }
            catch (Exception)
            {
                result = false;
            }

            return result;
        }
    }
}