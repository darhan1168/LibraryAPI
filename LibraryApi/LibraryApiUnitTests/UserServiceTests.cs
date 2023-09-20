using LibraryApi.Helpers;
using LibraryApi.Models;
using LibraryApi.Repositories.Interfaces;
using LibraryApi.Services.Implementations;
using LibraryApi.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Moq;

namespace LibraryApiUnitTests;

public class UserServiceTests
{
    [Fact]
    public async Task Register_NullUser_ResultFalse()
    {
        var userService = GetUserService();

        var registerResult = await userService.Register(null);
        
        Assert.False(registerResult.IsSuccessful);
        Assert.Equal("user not found", registerResult.Message);
    }
    
    [Fact]
    public async Task Register_InvalidEmail_ResultFalse()
    {
        var mockPasswordService = new Mock<IPasswordService>();
        var mockConfiguration = new Mock<IConfiguration>();
        
        var user = new User
        {
            Email = "existing_email@example.com",
            Password = "password"
        };

        var repositoryMock = new Mock<IBaseRepository<User>>();
        repositoryMock
            .Setup(r => r.GetAll())
            .Returns(new List<User> { user });

        var userService = new UserService(repositoryMock.Object, mockPasswordService.Object, mockConfiguration.Object);
        
        var result = await userService.Register(user);
        
        Assert.False(result.IsSuccessful);
        Assert.Equal("This email is not available", result.Message);
    }
    
    [Fact]
    public async Task Register_ValidUser_ResultTrue()
    {
        var repositoryMock = new Mock<IBaseRepository<User>>();
        var mockPasswordService = new Mock<IPasswordService>();
        var mockConfiguration = new Mock<IConfiguration>();
        
        var user = new User
        {
            Email = "not_existing_email@example.com",
            Password = "password"
        };
        
        mockPasswordService
            .Setup(p => p.HashPassword(It.IsAny<string>()))
            .Returns(BCrypt.Net.BCrypt.HashPassword(user.Password));

        repositoryMock
            .Setup(r => r.GetAll())
            .Returns(new List<User>{});
        
        repositoryMock
            .Setup(r => r.Add(It.IsAny<User>()))
            .ReturnsAsync(new Result<bool>(true));

        var userService = new UserService(repositoryMock.Object, mockPasswordService.Object, mockConfiguration.Object);
        
        var result = await userService.Register(user);
        
        Assert.True(result.IsSuccessful);
    }

    [Theory]
    [InlineData("", "")]
    [InlineData("", "password")]
    [InlineData("email", "")]
    [InlineData(null, null)]
    [InlineData(null, "password")]
    [InlineData("email", null)]
    public async Task Login_InvalidValues_ResultFalse(string email, string password)
    {
        var userService = GetUserService();

        var registerResult = await userService.Login(email, password);
        
        Assert.False(registerResult.IsSuccessful);
        Assert.Equal("Email or password is empty or null", registerResult.Message);
    }
    
    [Fact]
    public async Task Login_NotExistingEmail_ResultFalse()
    {
        var repositoryMock = new Mock<IBaseRepository<User>>();
        var mockPasswordService = new Mock<IPasswordService>();
        var mockConfiguration = new Mock<IConfiguration>();
        
        var user = new User
        {
            Email = "not_existing_email@example.com",
            Password = "password"
        };
        
        repositoryMock
            .Setup(r => r.GetAll())
            .Returns(new List<User>{});
        
        var userService = new UserService(repositoryMock.Object, mockPasswordService.Object, mockConfiguration.Object);
        
        var registerResult = await userService.Login(user.Email, user.Password);
        
        Assert.False(registerResult.IsSuccessful);
        Assert.Equal($"User with email {user.Email} not found", registerResult.Message);
    }
    
    [Fact]
    public async Task Login_IncorrectPassword_ResultFalse()
    {
        var repositoryMock = new Mock<IBaseRepository<User>>();
        var mockPasswordService = new Mock<IPasswordService>();
        var mockConfiguration = new Mock<IConfiguration>();
        
        var user = new User
        {
            Email = "existing_email@example.com",
            Password = "password"
        };
        
        repositoryMock
            .Setup(r => r.GetAll())
            .Returns(new List<User>{ user });
        
        mockPasswordService
            .Setup(p => p.VerifyPassword(It.IsAny<string>(), It.IsAny<string>()))
            .Returns(false);
        
        var userService = new UserService(repositoryMock.Object, mockPasswordService.Object, mockConfiguration.Object);
        
        var registerResult = await userService.Login(user.Email, "something_else");
        
        Assert.False(registerResult.IsSuccessful);
        Assert.Equal("Incorrect password", registerResult.Message);
    }
    
    [Fact]
    public async Task Login_CorrectValues_ResultFalse()
    {
        var repositoryMock = new Mock<IBaseRepository<User>>();
        var mockPasswordService = new Mock<IPasswordService>();
        var mockConfiguration = new Mock<IConfiguration>();
        
        var user = new User
        {
            Email = "existing_email@example.com",
            Password = "password"
        };
        
        repositoryMock
            .Setup(r => r.GetAll())
            .Returns(new List<User>{ user });
        
        mockPasswordService
            .Setup(p => p.VerifyPassword(It.IsAny<string>(), It.IsAny<string>()))
            .Returns(true);
        
        var userService = new UserService(repositoryMock.Object, mockPasswordService.Object, mockConfiguration.Object);
        
        var registerResult = await userService.Login(user.Email, user.Password);
        
        Assert.True(registerResult.IsSuccessful);
    }
    
    private UserService GetUserService()
    {
        var mockRepositoryService = new Mock<IBaseRepository<User>>();
        var mockPasswordService = new Mock<IPasswordService>();
        var mockConfiguration = new Mock<IConfiguration>();

        return new UserService(mockRepositoryService.Object, mockPasswordService.Object, mockConfiguration.Object);
    }
}