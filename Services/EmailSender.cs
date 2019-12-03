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
    public class EmailSender : IEmailSender
    {
        public void SendEmailConfirmation(string email, string callbackUrl)
        {
            var body = new StringBuilder();
            body.Append($"<a href='${callbackUrl}'>confirm your email</a>");
            sendMail("mohamed2511995@gmail.com", "AAaa123123", email, "confirm your email", body.ToString()); 
        }
        public void SendEmail(string email,string subject, string body)
        {
            sendMail("mohamed2511995@gmail.com", "AAaa123123", email, subject, body.ToString());
        }
        public  void sendMail(string from, string password, string email, string subject, string body)
        {
            MailMessage mail = new MailMessage();
            SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
            try
            {
                mail.From = new MailAddress(from);
                mail.To.Add(email);
                mail.Subject = subject;
                mail.Body = body; ;
                mail.IsBodyHtml = true;
                SmtpServer.Port = 587;
                SmtpServer.Credentials = new System.Net.NetworkCredential(from, password);
                SmtpServer.EnableSsl = true;
                SmtpServer.SendAsyncCancel();
                SmtpServer.Send(mail);
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
            MailMessage mail = (MailMessage)e.UserState;

            //write out the subject
            string subject = mail.Subject;

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
