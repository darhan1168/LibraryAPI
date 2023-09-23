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

        if (FindUserBooksForBook(user, book).Item1)
        {
            return new Result<bool>(false, $"This book already added in your books");
        }
        
        var addingResult = await _repository.Add(userBook);

        return !addingResult.IsSuccessful ? new Result<bool>(false, addingResult.Message) : new Result<bool>(true);
    }

    public async Task<Result<bool>> DeleteBookFromUser(Book book, User user)
    {
        if (book == null)
        {
            return new Result<bool>(false, $"{nameof(book)} not found");
        }
        if (user == null)
        {
            return new Result<bool>(false, $"{nameof(user)} not found");
        }

        var userBooks = user.UserBooks;

        if (userBooks == null)
        {
            return new Result<bool>(false, $"Have not any books in your library");
        }

        var userBook = userBooks.FirstOrDefault(ub => ub.BookId == book.Id);
        
        if (userBook == null)
        {
            return new Result<bool>(false, $"{nameof(book)}  not found  in your library");
        }

        var deletingResult = await _repository.Delete(userBook);

        return !deletingResult.IsSuccessful ? new Result<bool>(false, deletingResult.Message) : new Result<bool>(true);
    }

    private (bool, List<UserBook>?) FindUserBooksForBook(User user, Book book)
    {
        if (user.UserBooks == null)
        {
            return (false, null);
        }
    
        var userBooks = user.UserBooks.Where(ub => ub.BookId == book.Id && ub.UserId == user.Id).ToList();
    
        return (userBooks.Any(), userBooks.Any() ? userBooks : null);
    }
}