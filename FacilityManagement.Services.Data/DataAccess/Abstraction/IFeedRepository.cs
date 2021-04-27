using FacilityManagement.Services.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FacilityManagement.Services.Data.DataAccess.Abstraction
{
    public interface IFeedRepository: IGenericRepository<Category>
    {
        Task<Category> GetFeedByName(string name);
        Task<ICollection<Category>> GetCategoriesByPageNumber(int pageNumber, int per_page);
    }
}