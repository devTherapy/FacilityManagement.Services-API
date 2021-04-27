using FacilityManagement.Services.Data.DataAccess.Abstraction;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FacilityManagement.Services.Data.DataAccess.Implementation
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        private readonly DataContext _ctx;
        private readonly DbSet<TEntity> entities;
        public int TotalNumberOfItems { get; set; }
        public int TotalNumberOfPages { get; set; }

        public GenericRepository(DataContext ctx)
        {
            _ctx = ctx;
            entities = ctx.Set<TEntity>();
        }

        public IQueryable<TEntity> GetAll()
        {
            var result = entities.AsNoTracking();

            return result;
        }

        public async Task<ICollection<TEntity>> GetPaginated(int page, int per_page, IQueryable<TEntity> items)
        {
            TotalNumberOfItems = await items.CountAsync();

            TotalNumberOfPages = (int)Math.Ceiling(TotalNumberOfItems / (double)per_page);

            if (page > TotalNumberOfPages || page < 1)
            {
                return null;
            }
            var pagedItems = await items.Skip((page - 1) * per_page).Take(per_page).ToListAsync();
            return pagedItems;
        }

        public async Task<bool> Add(TEntity model)
        {
            entities.Add(model);
            return await _ctx.SaveChangesAsync() > 0;
        }

        public async Task<TEntity> GetById(object id)
        {
            return await entities.FindAsync(id);
        }

        public async Task<bool> Modify(TEntity entity)
        {
            entities.Update(entity);
            return await _ctx.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteById(object id)
        {
            var result = await GetById(id);
            entities.Remove(result);
            return await _ctx.SaveChangesAsync() > 0;
        }

        //public int GetPagesCount(int per_page)
        //{
        //    if (TotalNumberOfItems == 0)
        //        return 0;
        //    var count = TotalNumberOfItems / per_page;
        //    return TotalNumberOfItems % per_page != 0 ? count + 1 : count;
        //}
    }
}
