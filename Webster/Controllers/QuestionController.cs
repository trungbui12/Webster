using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Webster.Data;
using Webster.Models.Entities;
using Webster.Models.Enums;
using Webster.Models.ViewModels;

namespace Webster.Controllers
{
    public class QuestionController : Controller
    {
        private readonly ApplicationDbContext _context;

        public QuestionController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ================= LIST =================
        // ================= LIST WITH PAGINATION =================
        public IActionResult Index(int page = 1)
        {
            int pageSize = 5;

            var query = _context.Questions
                .Include(q => q.TestSection)
                .Include(q => q.Answers)
                .OrderByDescending(q => q.QuestionId);

            int totalItems = query.Count();
            int totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            var questions = query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            ViewBag.PageNumber = page;
            ViewBag.TotalPages = totalPages;

            return View(questions);
        }

        // ================= CREATE =================
        public IActionResult Create()
        {
            ViewBag.TestSections = _context.TestSections.ToList();

            var vm = new QuestionCreateVM
            {
                Answers = new List<AnswerCreateVM>
        {
            new(), new(), new(), new()
        }
            };

            return View(vm);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(QuestionCreateVM vm)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.TestSections = _context.TestSections.ToList();
                return View(vm);
            }

            var question = new Question
            {
                TestSectionId = vm.TestSectionId,
                Content = vm.Content,
                Score = vm.Score,
                Difficulty = vm.Difficulty,
                QuestionType = vm.QuestionType
            };

            _context.Questions.Add(question);
            _context.SaveChanges();

            // ===== SINGLE / MULTIPLE =====
            if (vm.QuestionType == QuestionType.SingleChoice ||
                vm.QuestionType == QuestionType.MultipleChoice ||
                vm.QuestionType == QuestionType.TrueFalse)
            {
                for (int i = 0; i < vm.Answers.Count; i++)
                {
                    bool isCorrect = false;

                    if (vm.QuestionType == QuestionType.SingleChoice)
                        isCorrect = (i == vm.CorrectAnswerId);

                    if (vm.QuestionType == QuestionType.MultipleChoice)
                        isCorrect = vm.CorrectAnswerIds.Contains(i);

                    _context.Answers.Add(new Answer
                    {
                        QuestionId = question.QuestionId,
                        Content = vm.Answers[i].Content,
                        IsCorrect = isCorrect
                    });
                }
            }

            // ===== TEXT =====
            if (vm.QuestionType == QuestionType.Text)
            {
                _context.Answers.Add(new Answer
                {
                    QuestionId = question.QuestionId,
                    Content = vm.CorrectTextAnswer!,
                    IsCorrect = true
                });
            }

            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }


        // ================= EDIT =================
        // ================= EDIT (GET) =================
        public IActionResult Edit(int id)
        {
            var q = _context.Questions
                .Include(x => x.Answers)
                .FirstOrDefault(x => x.QuestionId == id);

            if (q == null) return NotFound();

            var vm = new QuestionEditVM
            {
                QuestionId = q.QuestionId,
                TestSectionId = q.TestSectionId,
                Content = q.Content,
                Score = q.Score,
                Difficulty = q.Difficulty,
                QuestionType = q.QuestionType
            };

            // ===== TEXT =====
            if (q.QuestionType == QuestionType.Text)
            {
                vm.CorrectTextAnswer = q.Answers.FirstOrDefault()?.Content;
            }
            else
            {
                vm.Answers = q.Answers.Select(a => new AnswerEditVM
                {
                    AnswerId = a.AnswerId,
                    Content = a.Content
                }).ToList();

                // ===== SINGLE / TRUE FALSE =====
                if (q.QuestionType == QuestionType.SingleChoice ||
                    q.QuestionType == QuestionType.TrueFalse)
                {
                    vm.CorrectAnswerId = q.Answers
                        .FirstOrDefault(a => a.IsCorrect)?.AnswerId;
                }

                // ===== MULTIPLE =====
                if (q.QuestionType == QuestionType.MultipleChoice)
                {
                    vm.CorrectAnswerIds = q.Answers
                        .Where(a => a.IsCorrect)
                        .Select(a => a.AnswerId)
                        .ToList();
                }
            }

            ViewBag.TestSections = _context.TestSections.ToList();
            return View(vm);
        }


        // ================= EDIT (POST) =================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(QuestionEditVM vm)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.TestSections = _context.TestSections.ToList();
                return View(vm);
            }

            var q = _context.Questions
                .Include(x => x.Answers)
                .FirstOrDefault(x => x.QuestionId == vm.QuestionId);

            if (q == null) return NotFound();

            // ===== UPDATE BASIC INFO =====
            q.TestSectionId = vm.TestSectionId;
            q.Content = vm.Content;
            q.Score = vm.Score;
            q.Difficulty = vm.Difficulty;
            q.QuestionType = vm.QuestionType;

            // ===== TEXT QUESTION =====
            if (vm.QuestionType == QuestionType.Text)
            {
                // Xoá đáp án cũ
                _context.Answers.RemoveRange(q.Answers);

                // Thêm đáp án text mới
                _context.Answers.Add(new Answer
                {
                    QuestionId = q.QuestionId,
                    Content = vm.CorrectTextAnswer!,
                    IsCorrect = true
                });
            }
            else
            {
                // ===== UPDATE ANSWERS CONTENT =====
                foreach (var answer in q.Answers)
                {
                    var vmAns = vm.Answers
                        .FirstOrDefault(a => a.AnswerId == answer.AnswerId);

                    if (vmAns != null)
                        answer.Content = vmAns.Content;

                    answer.IsCorrect = false;
                }

                // ===== SINGLE / TRUE FALSE =====
                if (vm.QuestionType == QuestionType.SingleChoice ||
                    vm.QuestionType == QuestionType.TrueFalse)
                {
                    var correct = q.Answers
                        .FirstOrDefault(a => a.AnswerId == vm.CorrectAnswerId);

                    if (correct != null)
                        correct.IsCorrect = true;
                }

                // ===== MULTIPLE =====
                if (vm.QuestionType == QuestionType.MultipleChoice)
                {
                    foreach (var id in vm.CorrectAnswerIds)
                    {
                        var correct = q.Answers
                            .FirstOrDefault(a => a.AnswerId == id);

                        if (correct != null)
                            correct.IsCorrect = true;
                    }
                }
            }

            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }


        // ================= DELETE =================
        public IActionResult Delete(int id)
        {
            var q = _context.Questions.Find(id);
            if (q == null) return NotFound();

            return View(new QuestionDeleteVM
            {
                QuestionId = q.QuestionId,
                Content = q.Content
            });
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var q = _context.Questions
                .Include(x => x.Answers)
                .FirstOrDefault(x => x.QuestionId == id);

            if (q == null) return NotFound();

            _context.Answers.RemoveRange(q.Answers);
            _context.Questions.Remove(q);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
    }
}
