using LibraryApi.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryApi;

public class AppContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Book> Books { get; set; }
    
    public AppContext(DbContextOptions options) : base(options)
    {
    }
}