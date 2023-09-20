using LibraryApi.Helpers;
using LibraryApi.Models;
using LibraryApi.Repositories.Interfaces;
using LibraryApi.Services.Interfaces;

namespace LibraryApi.Services.Implementations;

public class BookService : GenericInterface<Book>, IBookService
{
    private readonly IBaseRepository<Book> _repository;

    public BookService(IBaseRepository<Book> repository) 
        : base(repository)
    {
        _repository = repository;
    }
    
    public async Task<Result<bool>> AddBook(Book book)
    {
        if (book == null)
        {
            return new Result<bool>(false, $"{nameof(book)} not found");
        }

        var addingResult = await _repository.Add(book);

        return !addingResult.IsSuccessful ? new Result<bool>(false, addingResult.Message) : new Result<bool>(true);
    }

    public async Task<Result<bool>> DeleteBook(int id)
    {
        var book = await GetById(id);
        
        if (book == null)
        {
            return new Result<bool>(false, $"{nameof(book)} not found");
        }
        
        var deletingResult = await _repository.Delete(book);

        return !deletingResult.IsSuccessful ? new Result<bool>(false, deletingResult.Message) : new Result<bool>(true);
    }

    public async Task<Result<bool>> UpdateBook(Book book)
    {
        if (book == null)
        {
            return new Result<bool>(false, $"{nameof(book)} not found");
        }
        
        var updatingResult = await _repository.Update(book);

        return !updatingResult.IsSuccessful ? new Result<bool>(false, updatingResult.Message) : new Result<bool>(true);
    }
}