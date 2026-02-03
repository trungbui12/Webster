using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Webster.Data;
using Webster.Models.Enums;

public class AccountController : Controller
{
    private readonly ApplicationDbContext _context;

    public AccountController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult Login() => View();

    [HttpPost]
    public async Task<IActionResult> Login(string username, string password)
    {
        // ================= MANAGER =================
        var manager = await _context.Managers
            .FirstOrDefaultAsync(m => m.Username == username);

        if (manager != null)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, manager.Username),
                new Claim(ClaimTypes.Role, "Manager")
            };

            await SignInUser(claims);
            return RedirectToAction("Dashboard", "Manager");
        }

        // ================= CANDIDATE =================
        var candidate = await _context.Candidates
            .FirstOrDefaultAsync(c => c.Username == username);

        if (candidate != null)
        {
            // ⚠ demo: chưa check hash
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, candidate.Username),
                new Claim(ClaimTypes.Role, "Candidate"),
                new Claim("CandidateId", candidate.CandidateId.ToString())
            };

            await SignInUser(claims);
            return RedirectToAction("Dashboard", "CandidateDashboard");
        }

        ViewBag.Error = "Invalid username or password";
        return View();
    }

    private async Task SignInUser(List<Claim> claims)
    {
        var identity = new ClaimsIdentity(
            claims,
            CookieAuthenticationDefaults.AuthenticationScheme);

        var principal = new ClaimsPrincipal(identity);

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            principal);
    }

    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync();
        return RedirectToAction("Login");
    }

    public IActionResult AccessDenied()
    {
        return Content("Access Denied");
    }
}
