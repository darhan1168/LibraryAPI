namespace LibraryApi.Models;

public class Book : BaseEntity
{
    public string Name { get; set; }
    public string Author { get; set; }
    public double Score { get; set; }
    public virtual List<User>? Users { get; set; }
}