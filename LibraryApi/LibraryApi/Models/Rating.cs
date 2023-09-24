namespace LibraryApi.Models;

public class Rating : BaseEntity
{
    public int BookId { get; set; }
    public virtual Book Book { get; set; }
    public double Value { get; set; }
}