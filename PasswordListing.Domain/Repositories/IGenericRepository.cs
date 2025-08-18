using System;
using System.Linq.Expressions;

namespace PasswordListing.Domain.Repositories;

public interface IGenericRepository<T> where T : class
{
    Task<IEnumerable<T>> GetAllAsync();
    Task<T?> GetByIdAsync(Guid id);
    IQueryable<T> Query();
    Task AddAsync(T entity);
    Task UpdateRangeAsync(IEnumerable<T> entities);
    Task UpdateAsync(T entity);
    Task DeleteAsync(T entity);
}
