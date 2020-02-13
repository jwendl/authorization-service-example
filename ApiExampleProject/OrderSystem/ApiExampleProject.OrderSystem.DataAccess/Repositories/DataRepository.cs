using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ApiExampleProject.Common.Pagination;
using ApiExampleProject.OrderSystem.DataAccess.Interfaces;
using ApiExampleProject.OrderSystem.DataAccess.Models;

namespace ApiExampleProject.OrderSystem.DataAccess.Repositories
{
    public class DataRepository<T>
        : IDataRepository<T>
        where T : BaseCosmosDocument
    {
        private readonly ICosmosClientWrapper<T> cosmosClientWrapper;

        public DataRepository(ICosmosClientWrapper<T> cosmosClientWrapper)
        {
            this.cosmosClientWrapper = cosmosClientWrapper;
        }

        public async Task<T> CreateItemAsync(T item)
        {
            return await cosmosClientWrapper.CreateItemAsync(item);
        }

        public async Task<IEnumerable<T>> ReadAllAsync()
        {
            return await Task.FromResult(new List<T>());
        }

        public async Task<IEnumerable<T>> ReadAllAsync(PaginationRequest paginationRequest)
        {
            _ = paginationRequest ?? throw new ArgumentNullException(nameof(paginationRequest));

            return await Task.FromResult(new List<T>());
        }
    }
}
