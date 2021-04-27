using FacilityManagement.Services.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FacilityManagement.Services.Data.DataAccess.Abstraction
{
    public interface IUserRepository:IGenericRepository<User>
    {
        Task<ICollection<User>> GetUsers(int page);
        Task<ICollection<User>> GetUsersBySquad(string squad, int page);
        Task<ICollection<User>> GetUsersByName(string name, int page);
        int perPage {get;}
       

    }
}
