using LibraryApi.Helpers;
using LibraryApi.Models;

namespace LibraryApi.Services.Interfaces;

public interface IBookService
{
    Task<Result<bool>> AddBook(Book book);
    Task<Result<bool>> DeleteBook(int id);
    Task<Result<bool>> UpdateBook(Book book);
    Task<Book> GetBookById(int id);
    List<Book> GetAllBooks();
}