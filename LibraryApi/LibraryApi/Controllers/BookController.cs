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
    
    public BookController(IBookService bookService)
    {
        _bookService = bookService;
    }
    
    [HttpGet("Books")]
    public IActionResult AllBooks()
    {
        var books = _bookService.GetAllBooks();

        if (!books.Any())
        {
            return BadRequest(new { Error = $"{nameof(books)} not added yet" });
        }
        
        return Ok(books);
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
}