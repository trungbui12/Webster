using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Webster.Data;
using Webster.Models.Entities;
using Webster.Models.Enums;
using Webster.Models.ViewModels;
using Microsoft.EntityFrameworkCore;
using Webster.Helpers;

namespace Webster.Controllers
{
    [Authorize(Roles = "Manager")]
    public class CandidateController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CandidateController(ApplicationDbContext context)
        {
            _context = context;
        }

        // LIST
        public IActionResult Index(int page = 1)
        {
            int pageSize = 5;

            var totalCandidates = _context.Candidates.Count();
            var totalPages = (int)Math.Ceiling((double)totalCandidates / pageSize);

            var candidates = _context.Candidates
                .OrderByDescending(c => c.CandidateId)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            ViewBag.PageNumber = page;
            ViewBag.TotalPages = totalPages;

            return View(candidates);
        }

        // CREATE - GET
        public IActionResult Create()
        {
            return View();
        }

        // CREATE - POST
        [HttpPost]
        public IActionResult Create(CandidateCreateVM model)
        {
            if (!ModelState.IsValid)
                return View(model);

            if (_context.Candidates.Any(c => c.Username == model.Username))
            {
                ModelState.AddModelError("Username", "Username already exists");
                return View(model);
            }

            var candidate = new Candidate
            {
                FullName = model.FullName,
                Email = model.Email,
                Phone = model.Phone,
                DateOfBirth = model.DateOfBirth,
                Username = model.Username,

                // ✅ BCrypt
                PasswordHash = PasswordHasher.Hash(model.Password),

                Status = CandidateStatus.Created,

                Education = new Education
                {
                    Degree = model.Degree,
                    University = model.University,
                    GraduationYear = model.GraduationYear
                },
                Experience = new Experience
                {
                    YearsOfExperience = model.YearsOfExperience,
                    PreviousCompany = model.PreviousCompany
                }
            };

            _context.Candidates.Add(candidate);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        // EDIT - GET
        public IActionResult Edit(int id)
        {
            var candidate = _context.Candidates
                .Include(c => c.Education)
                .Include(c => c.Experience)
                .FirstOrDefault(c => c.CandidateId == id);

            if (candidate == null) return NotFound();

            var vm = new CandidateEditVM
            {
                CandidateId = candidate.CandidateId,
                FullName = candidate.FullName,
                Email = candidate.Email,
                Phone = candidate.Phone,
                DateOfBirth = candidate.DateOfBirth,
                Status = candidate.Status,

                Degree = candidate.Education?.Degree ?? "",
                University = candidate.Education?.University ?? "",
                GraduationYear = candidate.Education?.GraduationYear ?? DateTime.Now.Year,

                YearsOfExperience = candidate.Experience?.YearsOfExperience ?? 0,
                PreviousCompany = candidate.Experience?.PreviousCompany ?? ""
            };

            return View(vm);
        }

        // EDIT - POST
        [HttpPost]
        public IActionResult Edit(CandidateEditVM vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            var candidate = _context.Candidates
                .Include(c => c.Education)
                .Include(c => c.Experience)
                .FirstOrDefault(c => c.CandidateId == vm.CandidateId);

            if (candidate == null) return NotFound();

            // BASIC
            candidate.FullName = vm.FullName;
            candidate.Email = vm.Email;
            candidate.Phone = vm.Phone;
            candidate.DateOfBirth = vm.DateOfBirth;
            candidate.Status = vm.Status;

            // EDUCATION
            if (candidate.Education == null)
                candidate.Education = new Education();

            candidate.Education.Degree = vm.Degree;
            candidate.Education.University = vm.University;
            candidate.Education.GraduationYear = vm.GraduationYear;

            // EXPERIENCE
            if (candidate.Experience == null)
                candidate.Experience = new Experience();

            candidate.Experience.YearsOfExperience = vm.YearsOfExperience;
            candidate.Experience.PreviousCompany = vm.PreviousCompany;

            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        // DELETE - GET
        public IActionResult Delete(int id)
        {
            var candidate = _context.Candidates.Find(id);
            if (candidate == null) return NotFound();

            return View(candidate);
        }

        // DELETE - POST
        [HttpPost]
        public IActionResult DeleteCandidate(int id)
        {
            var candidate = _context.Candidates.Find(id);

            if (candidate == null)
            {
                return Json(new { success = false });
            }

            _context.Candidates.Remove(candidate);
            _context.SaveChanges();

            return Json(new { success = true });
        }
    }
}