using LibraryApi.Enums;
using LibraryApi.Helpers;
using LibraryApi.Models;
using LibraryApi.Services.Interfaces;

namespace LibraryApi.Services.Implementations;

public class SeedValuesService : ISeedValuesService
{
    private readonly IUserService _userService;
    private readonly string? _adminPassword;
    private readonly string? _adminEmail;

    public SeedValuesService(IConfiguration configuration, IUserService userService)
    {
        _userService = userService;
        _adminPassword = configuration["AdminInfo:Password"];
        _adminEmail = configuration["AdminInfo:Email"];
    }
    
    public async Task<Result<bool>> AddSeedAdmin()
    {
        if (!_userService.HasAdminInDataBase())
        {
            if (_adminEmail == null || _adminPassword == null)
            {
                return new Result<bool>(false, "Email or password not found in a configuration");
            }

            var admin = new User()
            {
                Email = _adminEmail,
                Password = _adminPassword,
                Role = UserRole.Admin
            };

            var addingAdminResult = await _userService.Register(admin);
        
            if (!addingAdminResult.IsSuccessful)
            {
                return new Result<bool>(false, addingAdminResult.Message);
            }
        }
        
        return new Result<bool>(true);
    }
}