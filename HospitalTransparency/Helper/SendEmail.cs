using HospitalTransparency.Models;
using System;
using System.Linq;
using System.Net;
using System.Net.Mail;

namespace HospitalTransparency
{
    public class SendEmail
    {
        public static void Email(string emailId, string subject, string body)
        {
            using (var db = new HospitalTransparencyEntities())
            {
                
                    var mailSettings = db.MailSettings.FirstOrDefault(m => m.Id == "1");
                    if (mailSettings != null)
                    {
                        var mail = new MailMessage
                        {
                            IsBodyHtml = true,
                            Body = body,
                            From = new MailAddress(mailSettings.UserName, mailSettings.FromName)
                        };

                        mail.To.Add(emailId);
                        mail.Subject = "Forgot Password";

                        var cred = new NetworkCredential(mailSettings.UserName, mailSettings.Password);
                        var smtp = new SmtpClient(mailSettings.ServerHost, Convert.ToInt16(mailSettings.ServerPort))
                        {
                            EnableSsl = mailSettings.SSL,
                            DeliveryMethod = SmtpDeliveryMethod.Network,
                            UseDefaultCredentials = false,
                            Credentials = cred
                        };
                        smtp.SendMailAsync(mail);
                    }
                 
            }
        }
    }
}