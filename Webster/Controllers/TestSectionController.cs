using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Webster.Data;
using Webster.Models.Entities;
using Webster.Models.ViewModels;

namespace Webster.Controllers
{
    [Authorize(Roles = "Manager")]
    public class TestSectionController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TestSectionController(ApplicationDbContext context)
        {
            _context = context;
        }

        // LIST
        public IActionResult Index()
        {
            var sections = _context.TestSections.ToList();
            return View(sections);
        }

        // CREATE - GET
        public IActionResult Create()
        {
            return View();
        }

        // CREATE - POST
        [HttpPost]
        public IActionResult Create(TestSectionCreateVM vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            var section = new TestSection
            {
                SectionType = vm.SectionType,
                DurationInMinutes = vm.DurationInMinutes,
                TotalQuestions = vm.TotalQuestions
            };

            _context.TestSections.Add(section);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        // EDIT - GET
        public IActionResult Edit(int id)
        {
            var section = _context.TestSections.Find(id);
            if (section == null) return NotFound();

            var vm = new TestSectionEditVM
            {
                TestSectionId = section.TestSectionId,
                SectionType = section.SectionType,
                DurationInMinutes = section.DurationInMinutes,
                TotalQuestions = section.TotalQuestions
            };

            return View(vm);
        }

        // EDIT - POST
        [HttpPost]
        public IActionResult Edit(TestSectionEditVM vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            var section = _context.TestSections.Find(vm.TestSectionId);
            if (section == null) return NotFound();

            section.SectionType = vm.SectionType;
            section.DurationInMinutes = vm.DurationInMinutes;
            section.TotalQuestions = vm.TotalQuestions;

            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        // DELETE - GET
        public IActionResult Delete(int id)
        {
            var section = _context.TestSections.Find(id);
            if (section == null) return NotFound();

            return View(section);
        }

        // DELETE - POST
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var section = _context.TestSections.Find(id);
            if (section == null) return NotFound();

            _context.TestSections.Remove(section);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
    }
}
