using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Webster.Data;
using Webster.Models.Entities;

namespace Webster.Controllers
{
    [Authorize(Roles = "Manager")]
    public class ManagerController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ManagerController(ApplicationDbContext context)
        {
            _context = context;
        }

        // =========================
        // DASHBOARD
        // =========================

        public async Task<IActionResult> Dashboard()
        {
            var totalCandidates = await _context.Candidates.CountAsync();
            var totalQuestions = await _context.Questions.CountAsync();
            var totalSections = await _context.TestSections.CountAsync();
            var totalAnswers = await _context.Answers.CountAsync();

            var totalPassed = await _context.TestResults.CountAsync(x => x.IsPassed);
            var totalFailed = await _context.TestResults.CountAsync(x => !x.IsPassed);

            var totalFinished = await _context.TestResults.CountAsync();
            var totalInProgress = totalCandidates - totalFinished;

            ViewBag.TotalCandidates = totalCandidates;
            ViewBag.TotalQuestions = totalQuestions;
            ViewBag.TotalSections = totalSections;
            ViewBag.TotalAnswers = totalAnswers;

            ViewBag.TotalPassed = totalPassed;
            ViewBag.TotalFailed = totalFailed;
            ViewBag.TotalInProgress = totalInProgress;

            return View();
        }

        // =========================
        // PASSED CANDIDATES
        // =========================

        public async Task<IActionResult> PassedCandidates(string search, int page = 1)
        {
            int pageSize = 10;

            var query = _context.PassedCandidates
                .Include(x => x.Candidate)
                .ThenInclude(c => c.TestResult)
                .AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(x =>
                    x.Candidate.FullName.Contains(search) ||
                    x.Candidate.Email.Contains(search) ||
                    x.Candidate.Phone.Contains(search));
            }

            int totalItems = await query.CountAsync();

            var data = await query
                .OrderByDescending(x => x.PassedDate)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            ViewBag.TotalItems = totalItems;   // ⭐ THIẾU DÒNG NÀY
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalItems / pageSize);
            ViewBag.Search = search;

            return View(data);
        }

        // =========================
        // VIEW CANDIDATE PROFILE
        // =========================

        public async Task<IActionResult> ViewProfile(int id)
        {
            var candidate = await _context.Candidates
                .Include(x => x.Education)
                .Include(x => x.Experience)
                .Include(x => x.TestResult)
                .FirstOrDefaultAsync(x => x.CandidateId == id);

            if (candidate == null)
            {
                return NotFound();
            }

            return View(candidate);
        }

        // =========================
        // LOGOUT
        // =========================

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Account");
        }
    }
}