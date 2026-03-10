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

            // 2️⃣ Load Candidate
            var candidate = _context.Candidates
                .FirstOrDefault(c => c.CandidateId == candidateId);

            if (candidate == null)
                return Unauthorized();

            // 3️⃣ Load all test sections
            var sections = _context.TestSections
                .Include(x => x.Questions)
                .ToList();

            // 4️⃣ Load candidate sections
            var candidateSections = _context.CandidateTestSections
                .Where(x => x.CandidateId == candidateId)
                .ToList();

            // 5️⃣ Đảm bảo candidate có đủ section
            foreach (var section in sections)
            {
                bool exists = candidateSections
                    .Any(x => x.TestSectionId == section.TestSectionId);

                if (!exists)
                {
                    _context.CandidateTestSections.Add(new CandidateTestSection
                    {
                        CandidateId = candidateId,
                        TestSectionId = section.TestSectionId,
                        IsStarted = false,
                        IsCompleted = false,
                        Score = 0,
                        IsPassed = false
                    });
                }
            }

            _context.SaveChanges();

            // reload candidate sections
            candidateSections = _context.CandidateTestSections
                .Where(x => x.CandidateId == candidateId)
                .Include(x => x.TestSection)
                    .ThenInclude(x => x.Questions)
                .ToList();

            // 6️⃣ latest result
            var latestResult = _context.TestResults
                .Where(x => x.CandidateId == candidateId)
                .OrderByDescending(x => x.CompletedAt)
                .FirstOrDefault();

            // 7️⃣ map ViewModel
            var vm = new CandidateDashboardVM
            {
                CandidateId = candidate.CandidateId,
                FullName = candidate.FullName,
                Status = candidate.Status,
                TotalScore = latestResult?.TotalScore,
                IsPassed = latestResult?.IsPassed,

                TestSections = candidateSections
                    .OrderBy(x => x.TestSection.SectionType)
                    .Select(x => new CandidateTestSectionVM
                    {
                        TestSectionId = x.TestSectionId,
                        SectionType = x.TestSection.SectionType,

                        IsStarted = x.IsStarted,
                        IsCompleted = x.IsCompleted,

                        Score = x.Score,

                        StartedAt = x.StartedAt,
                        CompletedAt = x.CompletedAt,

                        // lấy từ database
                        QuestionCount = x.TestSection.TotalQuestions,

                        TimeLimit = x.TestSection.DurationInMinutes
                    })
                    .ToList()
            };

            return View(vm);
        }
    }
}