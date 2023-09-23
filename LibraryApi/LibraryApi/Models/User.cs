using LibraryApi.Enums;
using Microsoft.AspNetCore.Identity;

namespace LibraryApi.Models;

public class User : BaseEntity
{
    public string Email { get; set; }
    public string Password { get; set; }
    public UserRole Role { get; set; }
    public virtual List<UserBook>? UserBooks { get; set; }
}