using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Identity;

namespace AethersJournal.Pages
{
    public class ProcessLoginModel : PageModel
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<LoginModel> _logger;

        public ProcessLoginModel(SignInManager<User> signInManager, UserManager<User> userManager, ILogger<LoginModel> logger)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _logger = logger;
        }

        public async Task<IActionResult> OnGet(string? uid, string? token) {
            // check if parameters exist
            if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(uid)) return Redirect("/login"); 

            // find user by id and check if exists
            User? user = await _userManager.FindByIdAsync(uid);
            if (user == null) return Redirect("/login");
            
            _logger.Log(LogLevel.Information, "Process Login: UserID: " + uid);

            // validate token
            _logger.Log(LogLevel.Information, "Process Login: Token: " + token);
            bool isValid = await _userManager.VerifyUserTokenAsync(user, TokenOptions.DefaultProvider, "PreLogin", token);

            if (!isValid) return Redirect("/login");

            // login and redirect to journal page
            await _signInManager.SignInAsync(user, isPersistent: true);
            return Redirect("/");
        }
    }
}
