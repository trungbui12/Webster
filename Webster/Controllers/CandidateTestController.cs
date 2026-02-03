using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Webster.Data;
using Webster.Models.Entities;
using Webster.Models.ViewModels;

namespace Webster.Controllers
{
    public class CandidateTestController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CandidateTestController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ================= START =================
        public IActionResult Start(int testSectionId)
        {
            int candidateId = int.Parse(User.FindFirst("CandidateId")!.Value);

            // Lấy tất cả section theo thứ tự
            var allSections = _context.TestSections
                .OrderBy(x => x.TestSectionId)
                .ToList();

            // Tìm vị trí của section hiện tại
            int currentIndex = allSections
                .FindIndex(x => x.TestSectionId == testSectionId);

            if (currentIndex == -1)
                return NotFound();

            // Nếu không phải section đầu tiên
            if (currentIndex > 0)
            {
                var previousSectionId = allSections[currentIndex - 1].TestSectionId;

                var previousCandidateSection = _context.CandidateTestSections
                    .FirstOrDefault(x =>
                        x.CandidateId == candidateId &&
                        x.TestSectionId == previousSectionId);

                if (previousCandidateSection == null || !previousCandidateSection.IsCompleted)
                {
                    TempData["Error"] = "You must complete the previous test first.";
                    return RedirectToAction("Dashboard", "CandidateDashboard");
                }
            }

            var section = _context.CandidateTestSections
                .FirstOrDefault(x =>
                    x.CandidateId == candidateId &&
                    x.TestSectionId == testSectionId);

            if (section == null)
                return NotFound();

            if (!section.IsStarted)
            {
                section.IsStarted = true;
                section.StartedAt = DateTime.Now;
                _context.SaveChanges();
            }

            return RedirectToAction("Test", new { testSectionId });
        }

        // ================= LOAD TEST =================
        public IActionResult Test(int testSectionId)
        {
            var section = _context.TestSections
                .Include(s => s.Questions)
                    .ThenInclude(q => q.Answers)
                .FirstOrDefault(s => s.TestSectionId == testSectionId);

            if (section == null)
                return NotFound();

            var vm = new CandidateTestVM
            {
                TestSectionId = section.TestSectionId,
                SectionType = section.SectionType,
                DurationInMinutes = section.DurationInMinutes,
                Questions = section.Questions.Select(q => new QuestionVM
                {
                    QuestionId = q.QuestionId,
                    Content = q.Content,
                    Score = q.Score,
                    Answers = q.Answers.Select(a => new AnswerVM
                    {
                        AnswerId = a.AnswerId,
                        Content = a.Content
                    }).ToList()
                }).ToList()
            };

            return View(vm);
        }

        // ================= SUBMIT =================
        [HttpPost]
        public IActionResult Submit(CandidateTestVM model)
        {
            int candidateId = int.Parse(User.FindFirst("CandidateId")!.Value);

            int totalScore = 0;
            int maxScore = 0;

            foreach (var question in model.Questions)
            {
                var dbQuestion = _context.Questions
                    .Include(q => q.Answers)
                    .First(q => q.QuestionId == question.QuestionId);

                maxScore += dbQuestion.Score;

                if (question.SelectedAnswerId.HasValue)
                {
                    var selectedAnswer = dbQuestion.Answers
                        .First(a => a.AnswerId == question.SelectedAnswerId);

                    _context.CandidateAnswers.Add(new CandidateAnswer
                    {
                        CandidateId = candidateId,
                        QuestionId = question.QuestionId,
                        AnswerId = selectedAnswer.AnswerId,
                        AnsweredAt = DateTime.Now
                    });

                    if (selectedAnswer.IsCorrect)
                        totalScore += dbQuestion.Score;
                }
            }

            // Update section
            var section = _context.CandidateTestSections
                .First(x => x.CandidateId == candidateId
                         && x.TestSectionId == model.TestSectionId);

            section.Score = totalScore;
            section.IsCompleted = true;
            section.CompletedAt = DateTime.Now;

            // Section pass nếu đạt >= 60%
            double sectionPercent = (double)totalScore / maxScore * 100;
            section.IsPassed = sectionPercent >= 60;

            _context.SaveChanges();

            // Kiểm tra nếu đã hoàn thành tất cả section
            var allSections = _context.CandidateTestSections
                .Where(x => x.CandidateId == candidateId)
                .ToList();

            if (allSections.All(x => x.IsCompleted))
            {
                int finalScore = allSections.Sum(x => x.Score);

                // Tính tổng điểm tối đa toàn bài
                int maxTotalScore = _context.Questions.Sum(q => q.Score);

                double finalPercent = (double)finalScore / maxTotalScore * 100;
                bool finalPass = finalPercent >= 60;

                var existingResult = _context.TestResults
                    .FirstOrDefault(x => x.CandidateId == candidateId);

                if (existingResult == null)
                {
                    _context.TestResults.Add(new TestResult
                    {
                        CandidateId = candidateId,
                        TotalScore = finalScore,
                        IsPassed = finalPass,
                        CompletedAt = DateTime.Now
                    });
                }
                else
                {
                    existingResult.TotalScore = finalScore;
                    existingResult.IsPassed = finalPass;
                    existingResult.CompletedAt = DateTime.Now;
                }

                _context.SaveChanges();
            }

            return RedirectToAction("Dashboard", "CandidateDashboard");
        }
    }
}
