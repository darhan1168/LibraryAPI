using LibraryApi.Enums;
using LibraryApi.Helpers;
using LibraryApi.Models;
using LibraryApi.Repositories.Interfaces;
using LibraryApi.Services.Interfaces;

namespace LibraryApi.Services.Implementations;

public class UserService : IUserService
{
    private readonly IBaseRepository<User> _repository;
    private readonly IPasswordService _passwordService;

    public UserService(IBaseRepository<User> repository, IPasswordService passwordService, IConfiguration configuration)
    {
        _repository = repository;
        _passwordService = passwordService;
    }

    public async Task<Result<bool>> Register(User user)
    {
        if (user == null)
        {
            return new Result<bool>(false, $"{nameof(user)} not found");
        }

        if (IsFreeEmail(user.Email))
        {
            return new Result<bool>(false, "This email is not available");
        }

        var hashPassword = _passwordService.HashPassword(user.Password);
        user.Password = hashPassword;

        var addingResult = await _repository.Add(user);
        
        return !addingResult.IsSuccessful ? new Result<bool>(false, addingResult.Message) : new Result<bool>(true);
    }
    public bool HasAdminInDataBase()
    {
        var usersByEmail = _repository.GetAll().FirstOrDefault(u => u.Role == UserRole.Admin);

        return usersByEmail != null;
    }

    private bool IsFreeEmail(string email)
    {
        var usersByEmail = _repository.GetAll().FirstOrDefault(u => u.Email == email);

        return usersByEmail != null;
    }
}