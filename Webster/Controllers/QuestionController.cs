using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Webster.Data;
using Webster.Models.Entities;
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
        public IActionResult Index()
        {
            var questions = _context.Questions
                .Include(q => q.TestSection)
                .Include(q => q.Answers)
                .ToList();

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
                Difficulty = vm.Difficulty
            };

            _context.Questions.Add(question);
            _context.SaveChanges();

            // 🔥 lưu 4 đáp án
            for (int i = 0; i < vm.Answers.Count; i++)
            {
                _context.Answers.Add(new Answer
                {
                    QuestionId = question.QuestionId,
                    Content = vm.Answers[i].Content,
                    IsCorrect = (i == vm.CorrectAnswerId)
                });
            }

            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }


        // ================= EDIT =================
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

                // 🔥 lấy đáp án đúng hiện tại
                CorrectAnswerId = q.Answers.First(a => a.IsCorrect).AnswerId,

                Answers = q.Answers.Select(a => new AnswerEditVM
                {
                    AnswerId = a.AnswerId,
                    Content = a.Content
                }).ToList()
            };

            ViewBag.TestSections = _context.TestSections.ToList();
            return View(vm);
        }


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

            q.TestSectionId = vm.TestSectionId;
            q.Content = vm.Content;
            q.Score = vm.Score;
            q.Difficulty = vm.Difficulty;

            // 🔥 Update Answers
            foreach (var answer in q.Answers)
            {
                var vmAns = vm.Answers.First(a => a.AnswerId == answer.AnswerId);

                answer.Content = vmAns.Content;
                answer.IsCorrect = (answer.AnswerId == vm.CorrectAnswerId);
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
