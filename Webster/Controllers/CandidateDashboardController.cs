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

        public IActionResult Dashboard()
        {
            // 1️⃣ Lấy CandidateId từ Claims
            var claim = User.FindFirst("CandidateId");
            if (claim == null)
                return Unauthorized();

            int candidateId = int.Parse(claim.Value);

            // 2️⃣ Load candidate + navigation
            var candidate = _context.Candidates
                .Include(c => c.CandidateTestSections)
                    .ThenInclude(cts => cts.TestSection)
                .Include(c => c.TestResult)
                .FirstOrDefault(c => c.CandidateId == candidateId);

            if (candidate == null)
                return Unauthorized();

            // 3️⃣ Đảm bảo candidate có đủ tất cả test section
            var allSections = _context.TestSections.ToList();

            foreach (var section in allSections)
            {
                bool alreadyExists = candidate.CandidateTestSections
                    .Any(x => x.TestSectionId == section.TestSectionId);

                if (!alreadyExists)
                {
                    candidate.CandidateTestSections.Add(new CandidateTestSection
                    {
                        CandidateId = candidate.CandidateId,
                        TestSectionId = section.TestSectionId,
                        IsStarted = false,
                        IsCompleted = false,
                        Score = 0,
                        IsPassed = false
                    });
                }
            }

            _context.SaveChanges();

            // Reload navigation (đảm bảo đủ dữ liệu)
            _context.Entry(candidate)
                .Collection(c => c.CandidateTestSections)
                .Query()
                .Include(cts => cts.TestSection)
                .Load();

            // 4️⃣ Map sang ViewModel
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
