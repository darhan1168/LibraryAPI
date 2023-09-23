using LibraryApi.Helpers;
using LibraryApi.Models;
using LibraryApi.Repositories.Interfaces;
using LibraryApi.Services.Interfaces;

namespace LibraryApi.Services.Implementations;

public class UserBookService : IUserBookService
{
    private readonly IBaseRepository<UserBook> _repository;
    
    public UserBookService(IBaseRepository<UserBook> repository)
    {
        _repository = repository;
    }
    
    public async Task<Result<bool>> AddBookForUser(Book book, User user)
    {
        if (book == null)
        {
            return new Result<bool>(false, $"{nameof(book)} not found");
        }
        if (user == null)
        {
            return new Result<bool>(false, $"{nameof(user)} not found");
        }

        var userBook = new UserBook()
        {
            Book = book,
            User = user,
            BookId = book.Id,
            UserId = user.Id
        };

        if (HasUserThisUserBook(user, userBook))
        {
            return new Result<bool>(false, $"This book already added in your books");
        }
        
        var addingResult = await _repository.Add(userBook);

        return !addingResult.IsSuccessful ? new Result<bool>(false, addingResult.Message) : new Result<bool>(true);
    }

    private bool HasUserThisUserBook(User user, UserBook userBook)
    {
        if (user.UserBooks == null)
        {
            return false;
        }
        
        var result = user.UserBooks.Any(ub => ub.BookId == userBook.BookId && ub.UserId == userBook.UserId);

        return result;
    }
}