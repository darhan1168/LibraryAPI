using System.Security.Claims;
using LibraryApi.Controllers;
using LibraryApi.Enums;
using LibraryApi.Helpers;
using LibraryApi.Models;
using LibraryApi.Services.Interfaces;
using LibraryApi.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace LibraryApiUnitTests;

public class BookControllerTests : GenericControllerTestsSettings<BookController>
{
    [Fact]
    public void AllBooks_EmptyListOfBooks_BadRequest()
    {
        var mockService = new Mock<IBookService>();
        var userService = new Mock<IUserService>();
        var userBookService = new Mock<IUserBookService>();
        
        mockService.Setup(service => service.GetAllBooks())
                .Returns(new List<Book>());
        
        var controller = new BookController(mockService.Object, userService.Object, userBookService.Object);
        
        var result = controller.IndexBooks() as BadRequestObjectResult;

        Assert.Equal(400, result?.StatusCode);
    }
    
    [Fact]
    public void AllBooks_NotEmptyListOfBooks_Ok()
    {
        var mockService = new Mock<IBookService>();
        var userService = new Mock<IUserService>();
        var userBookService = new Mock<IUserBookService>();
        
        var book = new Book()
        {
            Name = "BookName",
            Author = "AuthorName"
        };
        
        mockService.Setup(service => service.GetAllBooks())
            .Returns(new List<Book> { book });
        
        var controller = new BookController(mockService.Object, userService.Object, userBookService.Object);
        
        var result = controller.IndexBooks() as OkObjectResult;

        Assert.NotNull(result);
        Assert.Equal(200, result.StatusCode);

        var books = result.Value as IEnumerable<Book>;
        Assert.NotNull(books);
    }

    [Fact]
    public async Task AddBook_ValidBook_Ok()
    {
        var mockService = new Mock<IBookService>();
        var userService = new Mock<IUserService>();
        var userBookService = new Mock<IUserBookService>();

        var bookViewModel = new BookViewModel()
        {
            Name = "BookName",
            Author = "AuthorName"
        };
        
        mockService.Setup(service => service.AddBook(It.IsAny<Book>()))
            .ReturnsAsync(new Result<bool>(true));
        
        var controller = new BookController(mockService.Object, userService.Object, userBookService.Object);
        
        SettingsHttpConnection(controller);
        
        var result = await controller.AddBook(bookViewModel) as OkResult;
        
        Assert.Equal(200, result?.StatusCode);
    }
    
    [Fact]
    public async Task DeleteBook_NotFoundBook_BadRequest()
    {
        var mockService = new Mock<IBookService>();
        var userService = new Mock<IUserService>();
        var userBookService = new Mock<IUserBookService>();

        mockService.Setup(service => service.GetBookById(It.IsAny<int>()))
            .ReturnsAsync((Book)null);
        
        var controller = new BookController(mockService.Object, userService.Object, userBookService.Object);
        
        SettingsHttpConnection(controller);
        
        var result = await controller.DeleteBook(1) as BadRequestObjectResult;
        
        Assert.Equal(400, result?.StatusCode);
    }
    
    [Fact]
    public async Task DeleteBook_FoundBook_Ok()
    {
        var mockService = new Mock<IBookService>();
        var userService = new Mock<IUserService>();
        var userBookService = new Mock<IUserBookService>();

        mockService.Setup(service => service.GetBookById(It.IsAny<int>()))
            .ReturnsAsync(new Book()
            {
                Id = 1
            });
        
        var controller = new BookController(mockService.Object, userService.Object, userBookService.Object);

        SettingsHttpConnection(controller);
        
        var result = await controller.DeleteBook(1) as OkResult;
        
        Assert.Equal(200, result?.StatusCode);
    }
}