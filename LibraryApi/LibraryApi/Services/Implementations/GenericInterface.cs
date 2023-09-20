using LibraryApi.Models;
using LibraryApi.Repositories.Interfaces;
using LibraryApi.Services.Interfaces;

namespace LibraryApi.Services.Implementations;

public class GenericInterface<T>: IGenericInterface<T> where T : BaseEntity
{
    private readonly IBaseRepository<T> _repository;

    protected GenericInterface(IBaseRepository<T> repository)
    {
        _repository = repository;
    }

    public async Task<T>? GetById(int id)
    {
        var entityById = await _repository.GetById(id);

        return entityById;
    }
}