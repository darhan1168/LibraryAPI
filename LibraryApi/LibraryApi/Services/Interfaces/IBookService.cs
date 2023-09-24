using LibraryApi.Enums;
using LibraryApi.Helpers;
using LibraryApi.Models;

namespace LibraryApi.Services.Interfaces;

public interface IBookService
{
    Task<Result<bool>> AddBook(Book book);
    Task<Result<bool>> DeleteBook(int id);
    Task<Result<bool>> UpdateBook(Book book);
    Task<Result<bool>> UpdateScoreForBookByUser(Book book, User user, int rating);
    Task<Book> GetBookById(int id);
    List<Book> GetBooksByUserId(int userId);
    List<Book> GetAllBooks(BookType? bookType);
}