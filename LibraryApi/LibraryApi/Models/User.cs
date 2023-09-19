using LibraryApi.Enums;

namespace LibraryApi.Models;

public class User : BaseEntity
{
    public string Email { get; set; }
    public string Password { get; set; }
    public UserRole Role { get; set; }
    public virtual List<Book>? Books { get; set; }
}