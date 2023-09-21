using System.Security.Claims;
using LibraryApi.Helpers;
using LibraryApi.Models;
using LibraryApi.Services.Interfaces;
using LibraryApi.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryApi.Controllers;

[AllowAnonymous]
[Route("[controller]")]
public class AccountController : Controller
{
    private readonly IUserService _userService;

    public AccountController(IUserService userService)
    {
        _userService = userService;
    }
    
    [HttpGet("Register")]
    public IActionResult Register()
    {
        return Ok();
    }
    
    [HttpPost("Register")]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (ModelState.IsValid)
        {
            var user = new User();
            model.MapTo(user);

            var addingResult = await _userService.Register(user);

            if (!addingResult.IsSuccessful)
            {
                return BadRequest(new { Error = addingResult.Message });
            }
            
            await Authenticate(model.Email);
            
            return Ok();
        }
        
        return BadRequest(new { Error = "Model is not valid" });
    }

    [HttpGet("Login")]
    public IActionResult Login()
    {
        return Ok();
    }
    
    [HttpPost("Login")]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (ModelState.IsValid)
        {
            var loginResult = await _userService.Login(model.Email, model.Password);
            
            if (!loginResult.IsSuccessful)
            {
                return BadRequest(new { Error = loginResult.Message });
            }
            
            await Authenticate(model.Email);
            
            return Ok();
        }
        
        return BadRequest(new { Error = "Model is not valid" });
    }
    
    [HttpPost("Logout")]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync();
        
        return Ok();
    }
    
    private async Task Authenticate(string email)
    {
        var claims = new List<Claim>
        {
            new (ClaimsIdentity.DefaultNameClaimType, email)
        };

        var id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);

        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
    }
}