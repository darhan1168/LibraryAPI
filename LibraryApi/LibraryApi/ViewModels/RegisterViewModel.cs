using LibraryApi.Enums;

namespace LibraryApi.ViewModels;

public class RegisterViewModel : BaseViewModel
{
    public string Email { get; set; }
    public string Password { get; set; }
    public UserRole Role { get; set; }
}