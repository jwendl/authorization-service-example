using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ApiExampleProject.Common.Pagination;
using ApiExampleProject.CustomerData.DataAccess.Models;

namespace ApiExampleProject.CustomerData.DataAccess.Interfaces
{
    public interface IDataRepository<T>
        where T : BaseDatabaseModel
    {
        Task<T> CreateItemAsync(T item);

        Task<IEnumerable<T>> ReadAllAsync();

        Task<IEnumerable<T>> ReadAllAsync(PaginationRequest paginationRequest);

        Task<T> ReadItemAsync(Guid id);

        Task<T> UpdateItemAsync(T item);

        Task DeleteItemAsync(Guid id);
    }
}
