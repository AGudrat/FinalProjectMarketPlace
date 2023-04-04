using MarketPlace.Web.Models;
using MarketPlace.Web.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace MarketPlace.Web.Controllers;

public class AuthController : Controller
{
    private readonly IIdentitiyService _identitiyService;

    public AuthController(IIdentitiyService ıdentitiyService)
    {
        _identitiyService = ıdentitiyService;
    }

    public IActionResult SignIn()
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> SignIn(SignInInput signInInput)
    {
        if (!ModelState.IsValid)
        {
            return View();
        }
        var response = await _identitiyService.SignIn(signInInput);
        if (!response.IsSuccessful)
        {
            response.Errors.ForEach(error =>
            {
                ModelState.AddModelError(String.Empty, error);
            });
            return View();
        }
        return RedirectToAction(nameof(Index), "Home");
    }
    public IActionResult SignUp()
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> SignUp(SignUpInput signUpInput)
    {
        if (!ModelState.IsValid)
        {
            return View();
        }
        var response = await _identitiyService.SignUp(signUpInput);
        if (!response.IsSuccessful)
        {
            response.Errors.ForEach(error =>
            {
                ModelState.AddModelError(String.Empty, error);
            });
            return View();
        }
        return RedirectToAction(nameof(Index), "Home");

    }
    public async Task<IActionResult> LogOut()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        await _identitiyService.RevokeRefreshToken();

        return RedirectToAction(nameof(HomeController.Index), "Home");
    }
}
