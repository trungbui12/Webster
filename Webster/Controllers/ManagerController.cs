using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Webster.Data;

namespace Webster.Controllers
{
    public class ManagerController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ManagerController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /Manager/Dashboard
        [Authorize(Roles = "Manager")]
        public IActionResult Dashboard()
        {
            // Simple statistics
            ViewBag.TotalCandidates = _context.Candidates.Count();
            ViewBag.TotalQuestions = _context.Questions.Count();
            ViewBag.TotalSections = _context.TestSections.Count();
            ViewBag.TotalAnswers = _context.Answers.Count();

            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Account");
        }
    }
}
