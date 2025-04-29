using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

[Authorize]
public class AccountController : Controller
{
    private readonly BarberShop.Services.IUserService _userService;

    public AccountController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    public IActionResult CompleteProfile()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CompleteProfile(ProfileViewModel model, IFormFile diplomaFile)
    {
        if (ModelState.IsValid)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _userService.CompleteProfileAsync(userId, model, diplomaFile);

            if (result.Success)
            {
                return RedirectToAction("Index", model.UserType);
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error);
            }
        }

        return View(model);
    }
}