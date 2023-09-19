using LibraryApi.Helpers;
using LibraryApi.Models;

namespace LibraryApi.Services.Interfaces;

public interface IUserService
{
   Task<Result<bool>> Register(User user);
   bool HasAdminInDataBase();
}