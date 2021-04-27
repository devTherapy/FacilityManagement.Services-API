using System.Threading.Tasks;

namespace FacilityManagement.Services.Core.Interfaces
{
   public interface IMailService
   {
        Task<bool> SendEmail(string recepient, string message, string msgSubject);
   }
}
