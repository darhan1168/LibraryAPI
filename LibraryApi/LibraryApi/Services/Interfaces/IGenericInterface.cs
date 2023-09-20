using LibraryApi.Models;
using LibraryApi.Repositories.Interfaces;

namespace LibraryApi.Services.Interfaces;

public interface IGenericInterface<T> where T : BaseEntity
{
    Task<T>? GetById(int id);
}