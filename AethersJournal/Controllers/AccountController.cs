using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AethersJournal.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountController : ControllerBase
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;

    public AccountController(UserManager<User> userManager, SignInManager<User> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    [HttpGet("signin")]
    public async Task<IActionResult> SignIn([FromQuery] string email, [FromQuery] string returnUrl = "/")
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null)
        {
            return RedirectToPage("/Error");
        }

        await _signInManager.SignInAsync(user, isPersistent: false);
        return Redirect(returnUrl);
    }
} 