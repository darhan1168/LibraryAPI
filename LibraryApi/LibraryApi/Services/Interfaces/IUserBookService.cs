using LibraryApi.Helpers;
using LibraryApi.Models;

namespace LibraryApi.Services.Interfaces;

public interface IUserBookService
{
    Task<Result<bool>> AddBookForUser(Book book, User user);
}