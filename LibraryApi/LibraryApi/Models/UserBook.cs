namespace LibraryApi.Models;

public class UserBook : BaseEntity
{
    public int UserId { get; set; }
    public virtual User User { get; set; }
    public int BookId { get; set; }
    public virtual Book Book { get; set; }
}