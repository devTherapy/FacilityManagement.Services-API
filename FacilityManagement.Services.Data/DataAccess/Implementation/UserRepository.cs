using FacilityManagement.Services.Data.DataAccess.Abstraction;
using FacilityManagement.Services.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FacilityManagement.Services.Data.DataAccess.Implementation
{
    public class UserRepository: GenericRepository<User>, IUserRepository
    {
        public int perPage {get;} = 10;
       
        public UserRepository(DataContext ctx): base(ctx)
        {
        }

        public async Task<ICollection<User>> GetUsers(int page)
        {
            var users = GetAll();
            var pagedUsers = await GetPaginated(page, perPage, users);
            return pagedUsers;

        }

        public async Task<ICollection<User>> GetUsersBySquad(string squad, int page)
        {
            var usersInSquad = GetAll().Where(u => u.Squad.ToLower() == squad.ToLower()).AsNoTracking();
            var pagedUsers = await GetPaginated(page, perPage, usersInSquad);
            return pagedUsers;
        }

        public Task<ICollection<User>> GetUsersByName(string name, int page)
        {
            var usersWithName = GetAll().Where(u => u.FirstName.ToLower().StartsWith(name.ToLower())
            || u.LastName.ToLower().StartsWith(name.ToLower())
            || u.FirstName.ToLower() + " " + u.LastName.ToLower()  == name.ToLower()).AsNoTracking();
            
            var pagedUsers = GetPaginated(page, perPage, usersWithName);
            return pagedUsers;
        }



    }
}
