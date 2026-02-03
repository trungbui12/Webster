using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Webster.Data;
using Webster.Models.Entities;
using Webster.Models.ViewModels;

namespace Webster.Controllers
{
    public class CandidateDashboardController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CandidateDashboardController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ✅ ACTION TÊN DASHBOARD
        public IActionResult Dashboard()
        {
            int candidateId = int.Parse(User.FindFirst("CandidateId")!.Value);

            var candidate = _context.Candidates
                .Include(c => c.CandidateTestSections)
                    .ThenInclude(cts => cts.TestSection)
                .Include(c => c.TestResult)
                .FirstOrDefault(c => c.CandidateId == candidateId);

            if (candidate == null)
                return Unauthorized();

            // ✅ AUTO CREATE 3 TEST SECTIONS IF NOT EXISTS
            if (!candidate.CandidateTestSections.Any())
            {
                var allSections = _context.TestSections.ToList();

                foreach (var section in allSections)
                {
                    candidate.CandidateTestSections.Add(new CandidateTestSection
                    {
                        CandidateId = candidate.CandidateId,
                        TestSectionId = section.TestSectionId,
                        IsStarted = false,
                        IsCompleted = false,
                        Score = 0
                    });
                }

                _context.SaveChanges();

                // reload
                _context.Entry(candidate)
                    .Collection(c => c.CandidateTestSections)
                    .Query()
                    .Include(cts => cts.TestSection)
                    .Load();
            }

            var vm = new CandidateDashboardVM
            {
                CandidateId = candidate.CandidateId,
                FullName = candidate.FullName,
                Status = candidate.Status,
                TotalScore = candidate.TestResult?.TotalScore,
                IsPassed = candidate.TestResult?.IsPassed,
                TestSections = candidate.CandidateTestSections
                    .OrderBy(x => x.TestSection.SectionType)
                    .Select(x => new CandidateTestSectionVM
                    {
                        TestSectionId = x.TestSectionId,
                        SectionType = x.TestSection.SectionType,
                        IsStarted = x.IsStarted,
                        IsCompleted = x.IsCompleted,
                        Score = x.Score,
                        StartedAt = x.StartedAt,
                        CompletedAt = x.CompletedAt
                    })
                    .ToList()
            };

            return View(vm);
        }

    }
}
