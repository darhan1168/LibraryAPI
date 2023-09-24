using LibraryApi.Enums;

namespace LibraryApi.Models;

public class Book : BaseEntity
{
    public string Name { get; set; }
    public string Author { get; set; }
    public BookType Type { get; set; }
    public double Score { get; set; }
    public virtual ICollection<Rating>? Ratings { get; set; } 
    public virtual List<UserBook>? UsersBooks { get; set; }
}