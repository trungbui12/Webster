using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Webster.Data;
using Webster.Models.Entities;
using Webster.Models.Enums;
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

            var allSections = _context.TestSections
                .OrderBy(x => x.TestSectionId)
                .ToList();

            int currentIndex = allSections
                .FindIndex(x => x.TestSectionId == testSectionId);

            if (currentIndex == -1)
                return NotFound();

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
            int candidateId = int.Parse(User.FindFirst("CandidateId")!.Value);

            var section = _context.TestSections
                .FirstOrDefault(s => s.TestSectionId == testSectionId);

            var candidateSection = _context.CandidateTestSections
                .FirstOrDefault(x =>
                    x.CandidateId == candidateId &&
                    x.TestSectionId == testSectionId);

            if (section == null || candidateSection == null)
                return NotFound();

            DateTime startTime = candidateSection.StartedAt ?? DateTime.Now;

            DateTime endTime = startTime.AddMinutes(section.DurationInMinutes);

            var questions = _context.Questions
                .Where(q => q.TestSectionId == testSectionId)
                .Include(q => q.Answers)
                .AsNoTracking()
                .ToList();

            var random = new Random();

            var randomQuestions = questions
                .OrderBy(x => random.Next())
                .Take(section.TotalQuestions)
                .ToList();

            foreach (var q in randomQuestions)
            {
                q.Answers = q.Answers
                    .OrderBy(a => random.Next())
                    .ToList();
            }

            var vm = new CandidateTestVM
            {
                TestSectionId = section.TestSectionId,
                SectionType = section.SectionType,
                DurationInMinutes = section.DurationInMinutes,
                EndTime = endTime,

                Questions = randomQuestions.Select(q => new QuestionVM
                {
                    QuestionId = q.QuestionId,
                    Content = q.Content,
                    Score = q.Score,
                    QuestionType = q.QuestionType,

                    Answers = q.Answers.Select(a => new AnswerVM
                    {
                        AnswerId = a.AnswerId,
                        Content = a.Content
                    }).ToList()

                }).ToList()
            };

            return View(vm);
        }

        [HttpPost]
        public IActionResult AutoSubmit(int testSectionId)
        {
            int candidateId = int.Parse(User.FindFirst("CandidateId")!.Value);

            var section = _context.CandidateTestSections
                .FirstOrDefault(x =>
                    x.CandidateId == candidateId &&
                    x.TestSectionId == testSectionId);

            if (section == null || section.IsCompleted)
                return Ok();

            section.IsCompleted = true;
            section.CompletedAt = DateTime.Now;

            _context.SaveChanges();

            return Ok();
        }
        // ================= SUBMIT =================
        [HttpPost]
        public IActionResult Submit(CandidateTestVM model)
        {
            int candidateId = int.Parse(User.FindFirst("CandidateId")!.Value);

            int totalScore = 0;
            int maxScore = 0;

            var questionIds = model.Questions.Select(q => q.QuestionId).ToList();

            // Load questions + answers trước
            var dbQuestions = _context.Questions
                .Include(q => q.Answers)
                .AsEnumerable()
                .Where(q => questionIds.Contains(q.QuestionId))
                .ToList();

            foreach (var question in model.Questions)
            {
                var dbQuestion = dbQuestions.First(q => q.QuestionId == question.QuestionId);

                maxScore += dbQuestion.Score;

                switch (dbQuestion.QuestionType)
                {
                    case QuestionType.SingleChoice:
                    case QuestionType.TrueFalse:

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

                        break;

                    case QuestionType.MultipleChoice:

                        var selectedIds = question.SelectedAnswerIds ?? new List<int>();

                        var correctIds = dbQuestion.Answers
                            .Where(a => a.IsCorrect)
                            .Select(a => a.AnswerId)
                            .ToList();

                        foreach (var ansId in selectedIds)
                        {
                            _context.CandidateAnswers.Add(new CandidateAnswer
                            {
                                CandidateId = candidateId,
                                QuestionId = question.QuestionId,
                                AnswerId = ansId,
                                AnsweredAt = DateTime.Now
                            });
                        }

                        if (correctIds.Count == selectedIds.Count &&
                            !correctIds.Except(selectedIds).Any())
                        {
                            totalScore += dbQuestion.Score;
                        }

                        break;

                    case QuestionType.Text:

                        _context.CandidateAnswers.Add(new CandidateAnswer
                        {
                            CandidateId = candidateId,
                            QuestionId = question.QuestionId,
                            TextAnswer = question.TextAnswer,
                            AnsweredAt = DateTime.Now
                        });

                        var correctText = dbQuestion.Answers
                            .FirstOrDefault(a => a.IsCorrect)?.Content
                            ?.Trim()
                            .ToLower();

                        if (!string.IsNullOrEmpty(correctText) &&
                            question.TextAnswer?.Trim().ToLower() == correctText)
                        {
                            totalScore += dbQuestion.Score;
                        }

                        break;
                }
            }

            var section = _context.CandidateTestSections
                .FirstOrDefault(x => x.CandidateId == candidateId
                                  && x.TestSectionId == model.TestSectionId);

            if (section == null)
            {
                section = new CandidateTestSection
                {
                    CandidateId = candidateId,
                    TestSectionId = model.TestSectionId,
                    IsStarted = true
                };

                _context.CandidateTestSections.Add(section);
            }

            section.Score = totalScore;
            section.IsCompleted = true;
            section.CompletedAt = DateTime.Now;

            double sectionPercent = maxScore == 0 ? 0 :
                (double)totalScore / maxScore * 100;

            section.IsPassed = sectionPercent >= 60;

            _context.SaveChanges();

            // ================= FINAL RESULT =================

            var allSections = _context.CandidateTestSections
                .Where(x => x.CandidateId == candidateId)
                .ToList();

            if (allSections.All(x => x.IsCompleted))
            {
                int finalScore = allSections.Sum(x => x.Score);
                int maxTotalScore = _context.Questions.Sum(q => q.Score);

                double finalPercent = maxTotalScore == 0 ? 0 :
                    (double)finalScore / maxTotalScore * 100;

                bool finalPass = finalPercent >= 60;

                var result = _context.TestResults
                    .FirstOrDefault(x => x.CandidateId == candidateId);

                if (result == null)
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
                    result.TotalScore = finalScore;
                    result.IsPassed = finalPass;
                    result.CompletedAt = DateTime.Now;
                }

                var candidate = _context.Candidates
                    .First(x => x.CandidateId == candidateId);

                if (finalPass)
                {
                    candidate.Status = CandidateStatus.Passed;

                    if (!_context.PassedCandidates.Any(p => p.CandidateId == candidateId))
                    {
                        _context.PassedCandidates.Add(new PassedCandidate
                        {
                            CandidateId = candidateId,
                            PassedDate = DateTime.Now
                        });
                    }
                }
                else
                {
                    candidate.Status = CandidateStatus.Failed;
                }

                _context.SaveChanges();
            }

            return RedirectToAction("Dashboard", "CandidateDashboard");
        }
    }
}