using System.Reflection.Metadata;
using LibraryApi.Controllers;
using LibraryApi.Models;
using LibraryApi.Repositories.Interfaces;
using LibraryApi.Services.Interfaces;
using LibraryApi.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace LibraryApiUnitTests;

public class AccountControllerTests
{
    [Fact]
    public async Task Register_NotValidModel_BadRequest()
    {
        var mockService = new Mock<IUserService>();
        
        var controller = new AccountController(mockService.Object);
        controller.ModelState.AddModelError("FieldName", "Error message");
        
        var result = await controller.Register(GetInvalidRegisterViewModel()) as BadRequestObjectResult;
        
        Assert.Equal(400, result?.StatusCode);
    }
    
    [Fact]
    public async Task Register_ValidModel_Ok()
    {
        var mockService = new Mock<IUserService>();
        
        var controller = new AccountController(mockService.Object);
        controller.ModelState.AddModelError("FieldName", "Error message");
        
        var result = await controller.Register(new RegisterViewModel()) as BadRequestObjectResult;
        
        Assert.Equal(400, result?.StatusCode);
    }

    private RegisterViewModel GetInvalidRegisterViewModel()
    {
        var registerViewModel = new RegisterViewModel()
        {
            Email = null,
            Password = null
        };

        return registerViewModel;
    }
}