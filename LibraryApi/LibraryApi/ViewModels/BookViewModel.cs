using LibraryApi.Enums;

namespace LibraryApi.ViewModels;

public class BookViewModel : BaseViewModel
{
    public string Name { get; set; }
    public string Author { get; set; }
    public BookType Type { get; set; }
    public double Score { get; set; }
}