using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FacilityManagement.Services.Data.DataAccess.Abstraction
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        int TotalNumberOfItems { get; set; }
        int TotalNumberOfPages { get; set; }
        IQueryable<TEntity> GetAll();
        Task<ICollection<TEntity>> GetPaginated(int page, int per_page, IQueryable<TEntity> items);
        Task<bool> Add(TEntity model);
        Task<TEntity> GetById(object Id);
        Task<bool> Modify(TEntity entity);
        Task<bool> DeleteById(object Id);
    }
}
