
using System.Text;
using System.Threading.Tasks;

namespace Atlob_Dent.Services
{
    public interface IEmailSender
    {
       void sendMail(string from, string password, string email, string subject, string body);
        void SendEmailConfirmation(string email, string callbackUrl);
        void SendEmail(string email, string subject, string body);
    }
}
