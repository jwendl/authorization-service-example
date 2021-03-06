﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ApiExampleProject.Common.Pagination;
using PolicyManager.DataAccess.Models;

namespace PolicyManager.DataAccess.Interfaces
{
    public interface IDataRepository<T>
        where T : BaseDatabaseModel
    {
        Task<T> CreateItemAsync(T item);

        Task<IEnumerable<T>> ReadAllAsync();

        Task<IEnumerable<T>> ReadAllAsync(params Expression<Func<T, object>>[] includes);

        Task<IEnumerable<T>> ReadAllAsync(PaginationRequest paginationRequest);

        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> queryExpression);

        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> queryExpression, params Expression<Func<T, object>>[] includes);

        Task<T> FindSingleAndIncludeAsync(Expression<Func<T, bool>> queryExpression, params Expression<Func<T, object>>[] includes);

        Task<T> ReadItemAsync(Guid id);

        Task<T> UpdateItemAsync(T item);

        Task DeleteItemAsync(Guid id);
    }
}
