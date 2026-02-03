using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Webster.Data;
using Webster.Models.ViewModels;

namespace Webster.Controllers
{
    public class ManagerCandidateController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ManagerCandidateController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            int totalSections = _context.TestSections.Count();

            var candidates = _context.Candidates
                .Include(c => c.CandidateTestSections)
                .Include(c => c.TestResult)
                .Select(c => new ManagerCandidateListVM
                {
                    CandidateId = c.CandidateId,
                    FullName = c.FullName,
                    Email = c.Email,
                    Status = c.Status.ToString(),

                    CompletedSections = c.CandidateTestSections
                        .Count(x => x.IsCompleted),

                    TotalSections = totalSections,

                    TotalScore = c.TestResult != null
                        ? c.TestResult.TotalScore
                        : null,

                    IsPassed = c.TestResult != null
                        ? c.TestResult.IsPassed
                        : null,

                    CompletedAt = c.TestResult != null
                        ? c.TestResult.CompletedAt
                        : null
                })
                .OrderByDescending(c => c.CompletedAt)
                .ToList();

            return View(candidates);
        }
    }
}