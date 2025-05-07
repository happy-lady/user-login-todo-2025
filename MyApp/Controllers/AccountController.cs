using Microsoft.AspNetCore.Mvc;
using MyApp.Data;
using MyApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;

namespace MyApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _context;

        public AccountController(AppDbContext context)
        {
            _context = context;
        }

        // GET: /Login
        public IActionResult Login()
        {
            return View();
        }

        // POST: /Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(UserLoginModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _context.Users
                    .FirstOrDefaultAsync(u => u.Username == model.Username);

                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return View(model);
                }

                // Authentication and setting up cookies
                var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = model.RememberMe // Optional: remember user after closing browser
                };

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                                              new ClaimsPrincipal(claimsIdentity), authProperties);

                return RedirectToAction("Index", "Home"); // Redirect to the home page after login
            }

            return View(model);
        }

        // GET: /Logout
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }

        // GET: /Register
        public IActionResult Register()
        {
            return View();
        }

        // POST: /Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(User model)
        {
            if (ModelState.IsValid)
            {
                // Check if the username already exists
                var existingUser = await _context.Users
                    .FirstOrDefaultAsync(u => u.Username == model.Username);

                if (existingUser != null)
                {
                    ModelState.AddModelError("Username", "Username is already taken.");
                    return View(model);
                }

                // Add user to the database
                _context.Users.Add(model);
                await _context.SaveChangesAsync();

                // Redirect to Login after successful registration
                return RedirectToAction("Login", "Login");
            }

            return View(model);
        }
    }

}
