using LibraryApi.Helpers;
using LibraryApi.Models;

namespace LibraryApi.Services.Interfaces;

public interface IUserService
{
   Task<Result<bool>> Register(User user);
   Task<Result<bool>> Login(string email, string password);
   User? GetUserByEmail(string email);
   bool HasAdminInDataBase();
}