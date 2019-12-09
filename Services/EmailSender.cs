using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Atlob_Dent.Services
{
    // This class is used by the application to send email for account confirmation and password reset.
    // For more details see https://go.microsoft.com/fwlink/?LinkID=532713
    public class EmailSender:IEmailSender
    {
        public async Task SendEmailAsync(string email, string subject, string body)
        {
            MailMessage mail = new MailMessage();
            SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
            try
            {               
                SmtpServer.Port = 587;
                SmtpServer.UseDefaultCredentials = false;
                SmtpServer.Credentials = new System.Net.NetworkCredential(GlobalVariables.EmailConfigObject.from, GlobalVariables.EmailConfigObject.password);
                SmtpServer.SendCompleted += SmtpClient_OnCompleted;
                SmtpServer.EnableSsl = true;
                if (GlobalVariables.EmailConfigObject.writeAsFile)
                {
                    SmtpServer.DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory;
                    //SmtpServer.PickupDirectoryLocation = "file directory";
                    SmtpServer.EnableSsl = false;
                    mail.BodyEncoding = Encoding.ASCII;
                }
                mail.From = new MailAddress(GlobalVariables.EmailConfigObject.from);
                mail.To.Add(email);
                mail.Subject = subject;
                mail.Body = body;
                mail.IsBodyHtml = true;

                await SmtpServer.SendMailAsync(mail);
            }
            catch (SmtpFailedRecipientException ex)
            {
                new Task(() =>
                {
                    SmtpServer.Send(mail);
                }).Start();
            }
            catch (Exception ex)
            {
            }
        }
        private  void SmtpClient_OnCompleted(object sender, AsyncCompletedEventArgs e)
        {
            //Get the Original MailMessage object
            var mess=e.UserState;


            if (e.Cancelled)
            {
                //  return false;
            }
            if (e.Error != null)
            {
                // return false;
            }
            else
            {
                string s = "";
                // return true;
            }
        }
    }
   
}
