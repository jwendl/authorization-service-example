using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiExampleProject.Common.Pagination;
using ApiExampleProject.CustomerData.DataAccess.Interfaces;
using ApiExampleProject.CustomerData.DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiExampleProject.CustomerData.DataAccess.Repositories
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

        public async Task<IEnumerable<T>> ReadAllAsync(PaginationRequest paginationRequest)
        {
            _ = paginationRequest ?? throw new ArgumentNullException(nameof(paginationRequest));

            return await dataContext.Set<T>()
                .Take(paginationRequest.PageSize)
                .Skip((paginationRequest.PageNumber - 1) * paginationRequest.PageSize)
                .ToListAsync();
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
