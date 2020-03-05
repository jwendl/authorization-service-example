using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ApiExampleProject.Common.Pagination;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using PolicyManager.DataAccess.Interfaces;
using PolicyManager.DataAccess.Models;

namespace PolicyManager.DataAccess.Repositories
{
    public class DataRepository<T>
        : IDataRepository<T>
        where T : BaseDatabaseModel
    {
        private readonly IDataContext dataContext;

        public DataRepository(IDataContext dataContext)
        {
            this.dataContext = dataContext ?? throw new ArgumentNullException(nameof(dataContext));
        }

        public async Task<T> CreateItemAsync(T item)
        {
            var entityEntry = await dataContext.AddAsync(item);
            await dataContext.SaveChangesAsync();
            return entityEntry.Entity;
        }

        public async Task<IEnumerable<T>> ReadAllAsync()
        {
            return await dataContext.Set<T>()
                .ToListAsync();
        }

        public async Task<IEnumerable<T>> ReadAllAsync(params Expression<Func<T, object>>[] includes)
        {
            if (!(dataContext.Set<T>() is IIncludableQueryable<T, object> query)) return new List<T>();

            foreach (var include in includes)
            {
                query = query
                    .Include(include);
            }

            var results = await query.ToListAsync();
            return results.AsEnumerable();
        }

        public async Task<IEnumerable<T>> ReadAllAsync(PaginationRequest paginationRequest)
        {
            _ = paginationRequest ?? throw new ArgumentNullException(nameof(paginationRequest));

            return await dataContext.Set<T>()
                .Take(paginationRequest.PageSize)
                .Skip((paginationRequest.PageNumber - 1) * paginationRequest.PageSize)
                .ToListAsync();
        }

        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> queryExpression)
        {
            return await dataContext.Set<T>()
                .Where(queryExpression)
                .ToListAsync();
        }

        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> queryExpression, params Expression<Func<T, object>>[] includes)
        {
            var query = dataContext.Set<T>()
                .Where(queryExpression);

            foreach (var include in includes)
            {
                query = query
                    .Include(include);
            }

            return await query.ToListAsync();
        }

        public async Task<T> FindSingleAndIncludeAsync(Expression<Func<T, bool>> queryExpression, params Expression<Func<T, object>>[] includes)
        {
            var query = dataContext.Set<T>()
                .Where(queryExpression);

            foreach (var include in includes)
            {
                query = query
                    .Include(include);
            }

            return await query.SingleAsync();
        }

        public async Task<T> ReadItemAsync(Guid id)
        {
            return await dataContext.Set<T>()
                .Where(e => e.Id == id)
                .SingleOrDefaultAsync();
        }

        public async Task<T> UpdateItemAsync(T item)
        {
            dataContext.Set<T>().Update(item);
            await dataContext.SaveChangesAsync();
            return item;
        }

        public async Task DeleteItemAsync(Guid id)
        {
            var item = await ReadItemAsync(id);
            dataContext.Set<T>().Remove(item);
            await dataContext.SaveChangesAsync();
        }
    }
}
