using LibraryApi.CustomAttributes;
using LibraryApi.Helpers;
using LibraryApi.Models;
using LibraryApi.Services.Interfaces;
using LibraryApi.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryApi.Controllers;

[Authorize]
[Route("[controller]")]
public class BookController : Controller
{
    private readonly IBookService _bookService;
    private readonly IUserService _userService;
    private readonly IUserBookService _userBookService;

    public BookController(IBookService bookService, IUserService userService, IUserBookService userBookService)
    {
        _bookService = bookService;
        _userService = userService;
        _userBookService = userBookService;
    }
    
    [HttpGet("Books")]
    public IActionResult IndexBooks()
    {
        var books = _bookService.GetAllBooks();

        if (!books.Any())
        {
            return BadRequest(new { Error = $"{nameof(books)} not added yet" });
        }
        
        return Ok(books);
    }
    
    [HttpGet("UserBooks")]
    public IActionResult IndexBooksForUser()
    {
        var userEmail = User.Identity?.Name;

        if (userEmail == null)
        {
            return BadRequest(new { Error = "User is not authorize" });
        }

        var user = _userService.GetUserByEmail(userEmail);
        
        if (user == null)
        {
            return BadRequest(new { Error = "User is not found" });
        }
        
        var booksForUser = _bookService.GetBooksByUserId(user.Id);

        if (!booksForUser.Any())
        {
            return BadRequest(new { Error = $"Books not added yet" });
        }

        var bookViewModels = booksForUser.Select(book => 
        {
            var bookViewModel = new BookViewModel();
            book.MapTo(bookViewModel);
            
            return bookViewModel;
        }).ToList();
        
        return Ok(bookViewModels);
    }
    
    [RoleAuthorize("Admin")]
    [HttpGet("AddBook")]
    public IActionResult AddBook()
    {
        return Ok();
    }
    
    [RoleAuthorize("Admin")]
    [HttpPost("AddBook")]
    public async Task<IActionResult> AddBook(BookViewModel model)
    {
        if (ModelState.IsValid)
        {
            var book = new Book();
            model.MapTo(book);

            var addingResult = await _bookService.AddBook(book);

            if (!addingResult.IsSuccessful)
            {
                return BadRequest(new { Error = addingResult.Message });
            }
        
            return Ok();
        }
        
        return BadRequest(new { Error = "Model is not valid" });
    }

    [RoleAuthorize("Admin")]
    [HttpGet("Delete/{id:int}")]
    public async Task<IActionResult> DeleteBook(int id)
    {
        var book = await _bookService.GetBookById(id);

        if (book == null)
        {
            return BadRequest(new { Error = $"{nameof(book)} not found" });
        }
        
        return Ok();
    }
    
    [RoleAuthorize("Admin")]    
    [HttpPost("Delete/{id:int}")]
    [ActionName("DeleteBook")]
    public async Task<IActionResult> ConfirmDeleteBook(int id)
    {
        var book = await _bookService.GetBookById(id);

        if (book == null)
        {
            return BadRequest(new { Error = $"{nameof(book)} not found" });
        }

        var deletionResult = await _bookService.DeleteBook(id);

        if (!deletionResult.IsSuccessful)
        {
            return BadRequest(new { Error = deletionResult.Message });
        }

        return Ok();
    }
    
    [RoleAuthorize("Admin")]
    [HttpGet("Update/{id:int}")]
    public async Task<IActionResult> UpdateBook(int id)
    {
        var book = await _bookService.GetBookById(id);

        if (book == null)
        {
            return BadRequest(new { Error = $"{nameof(book)} not found" });
        }
        
        return Ok();
    }
    
    [RoleAuthorize("Admin")]
    [HttpPost("Update")]
    public async Task<IActionResult> UpdateBook(BookViewModel model)
    {
        if (ModelState.IsValid)
        {
            var book = new Book();
            model.MapTo(book);

            var updatingResult = await _bookService.UpdateBook(book);

            if (!updatingResult.IsSuccessful)
            {
                return BadRequest(new { Error = updatingResult.Message });
            }
        
            return Ok();
        }
        
        return BadRequest(new { Error = "Model is not valid" });
    }

    [HttpPost("AddBookForUser")]
    public async Task<IActionResult> AddBookForUser(int id)
    {
        var book = await _bookService.GetBookById(id);
        
        if (book == null)
        {
            return BadRequest(new { Error = "Book is not found" });
        }
        
        var userEmail = User.Identity?.Name;

        if (userEmail == null)
        {
            return BadRequest(new { Error = "User is not authorize" });
        }

        var user = _userService.GetUserByEmail(userEmail);
        
        if (user == null)
        {
            return BadRequest(new { Error = "User is not found" });
        }

        var addingBookForUserResult = await _userBookService.AddBookForUser(book, user);
        
        if (!addingBookForUserResult.IsSuccessful)
        {
            return BadRequest(new { Error = addingBookForUserResult.Message });
        }
        
        return Ok();
    }

    [HttpPost("DeleteYourBook")]
    public async Task<IActionResult> DeleteBookFromUser(int id)
    {
        var book = await _bookService.GetBookById(id);
        
        if (book == null)
        {
            return BadRequest(new { Error = "Book is not found" });
        }
        
        var userEmail = User.Identity?.Name;

        if (userEmail == null)
        {
            return BadRequest(new { Error = "User is not authorize" });
        }

        var user = _userService.GetUserByEmail(userEmail);
        
        if (user == null)
        {
            return BadRequest(new { Error = "User is not found" });
        }
        
        var deletingBookFromUserResult = await _userBookService.DeleteBookFromUser(book, user);
        
        if (!deletingBookFromUserResult.IsSuccessful)
        {
            return BadRequest(new { Error = deletingBookFromUserResult.Message });
        }
        
        return Ok();
    }

    [HttpPost("AddRate/{rating:int}/ForBook/{id:int}")]
    public async Task<IActionResult> AddRateForBook(int id, int rating)
    {
        var book = await _bookService.GetBookById(id);
        
        if (book == null)
        {
            return BadRequest(new { Error = "Book is not found" });
        }

        var userEmail = User.Identity?.Name;

        if (userEmail == null)
        {
            return BadRequest(new { Error = "User is not authorize" });
        }

        var user = _userService.GetUserByEmail(userEmail);
        
        if (user == null)
        {
            return BadRequest(new { Error = "User is not found" });
        }
        
        var updatingScoreResult = await _bookService.UpdateScoreForBookByUser(book, user, rating);
        
        if (!updatingScoreResult.IsSuccessful)
        {
            return BadRequest(new { Error = updatingScoreResult.Message });
        }
        
        return Ok();
    }
}