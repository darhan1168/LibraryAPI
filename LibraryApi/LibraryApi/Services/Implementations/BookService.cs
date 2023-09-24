using LibraryApi.Helpers;
using LibraryApi.Models;
using LibraryApi.Repositories.Interfaces;
using LibraryApi.Services.Interfaces;

namespace LibraryApi.Services.Implementations;

public class BookService : IBookService
{
    private readonly IBaseRepository<Book> _repository;
    private readonly IUserBookService _userBookService;

    public BookService(IBaseRepository<Book> repository, IUserBookService userBookService)
    {
        _repository = repository;
        _userBookService = userBookService;
    }
    
    public async Task<Result<bool>> AddBook(Book book)
    {
        if (book == null)
        {
            return new Result<bool>(false, $"{nameof(book)} not found");
        }

        book.Score = 0;

        var addingResult = await _repository.Add(book);

        return !addingResult.IsSuccessful ? new Result<bool>(false, addingResult.Message) : new Result<bool>(true);
    }

    public async Task<Result<bool>> DeleteBook(int id)
    {
        var book = await GetBookById(id);
        
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

    public async Task<Result<bool>> UpdateScoreForBookByUser(Book book, User user, int rating)
    {
        var validationErrors = ValidateUpdateInput(book, user, rating);

        if (validationErrors.Any())
        {
            return new Result<bool>(false, string.Join("; ", validationErrors));
        }

        var addingRateResult = await AddRatingForBook(book, user, rating);

        if (!addingRateResult.IsSuccessful)
        {
            return new Result<bool>(false, addingRateResult.Message);
        }

        book.Score = CalculateAverageRating(book);

        var updatingResult = await UpdateBook(book);

        return !updatingResult.IsSuccessful ? new Result<bool>(false, updatingResult.Message) : new Result<bool>(true);
    }

    public async Task<Book> GetBookById(int id)
    {
        var book = await _repository.GetById(id);

        return book;
    }

    public List<Book> GetBooksByUserId(int userId)
    {
        var booksByUserId = _repository.GetAll()
            .Where(b => b.UsersBooks.Any(ub => ub.UserId == userId))
            .ToList();
        
        return booksByUserId;
    }

    public List<Book> GetAllBooks()
    {
        var books = _repository.GetAll();

        return books;
    }

    private async Task<Result<bool>> AddRatingForBook(Book book, User user, int rating)
    {
        if (book == null)
        {
            return new Result<bool>(false, $"{nameof(book)} not found");
        }
        
        var rate = new Rating()
        {
            Value = rating,
            Book = book,
            BookId = book.Id
        };
        
        if (book.Ratings == null)
        {
            return new Result<bool>(false, $"{nameof(book.Ratings)} not found");
        }
        
        book.Ratings.Add(rate);

        var updatingResult = await UpdateBook(book);

        if (!updatingResult.IsSuccessful)
        {
            return new Result<bool>(false, updatingResult.Message);
        }

        var updatingUserBookResult = await _userBookService.UpdateTrueRatedByUser(book, user);
        
        if (!updatingUserBookResult.IsSuccessful)
        {
            return new Result<bool>(false, updatingUserBookResult.Message);
        }
        
        return new Result<bool>(true);
    }

    private List<string> ValidateUpdateInput(Book book, User user, int rating)
    {
        const int minRating = 1;
        const int maxRating = 5;
        
        var errors = new List<string>();

        if (book == null)
        {
            errors.Add($"{nameof(book)} not found");
        }

        if (user == null)
        {
            errors.Add($"{nameof(user)} not found");
        }

        if (user.UserBooks == null)
        {
            errors.Add($"{nameof(user.UserBooks)} not found");
        }

        if (!user.UserBooks.Any(ub => ub.BookId == book.Id))
        {
            errors.Add($"You do not have this book");
        }

        if (user.UserBooks.Any(ub => ub.BookId == book.Id && (bool)ub.RatedByUser))
        {
            errors.Add($"This book \"{book.Name}\" already evaluated");
        }

        if (rating is < minRating or > maxRating)
        {
            errors.Add("Incorrect rating's value. Must be 1-5");
        }

        return errors;
    }

    private double CalculateAverageRating(Book book)
    {
        if (book.Ratings == null || book.Ratings.Count == 0)
        {
            return 0; 
        }

        return book.Ratings.Select(r => r.Value).Average();
    }
}