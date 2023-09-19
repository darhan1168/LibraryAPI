using LibraryApi.Helpers;
using LibraryApi.Models;

namespace LibraryApi.Repositories.Interfaces;

public interface IBaseRepository<T> where T : BaseEntity
{
    Task<T> GetById(int id);
    Task<List<T>> GetAll();
    Task<Result<bool>> Add(T entity);
    Task<Result<bool>> Delete(T entity);
    Task<Result<bool>> Update(T entity);
}