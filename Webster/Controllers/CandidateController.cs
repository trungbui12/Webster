using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;
using Webster.Data;
using Webster.Models.Entities;
using Webster.Models.Enums;
using Webster.Models.ViewModels;

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
        public IActionResult Index()
        {
            var candidates = _context.Candidates.ToList();
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
                PasswordHash = HashPassword(model.Password),
                Status = CandidateStatus.Created
            };

            _context.Candidates.Add(candidate);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        // EDIT - GET
        // EDIT - GET
        public IActionResult Edit(int id)
        {
            var candidate = _context.Candidates.Find(id);
            if (candidate == null) return NotFound();

            var vm = new CandidateEditVM
            {
                CandidateId = candidate.CandidateId,
                FullName = candidate.FullName,
                Email = candidate.Email,
                Phone = candidate.Phone,
                DateOfBirth = candidate.DateOfBirth,
                Status = candidate.Status
            };

            return View(vm); // ✅ TRẢ VIEWMODEL
        }


        // EDIT - POST
        [HttpPost]
        public IActionResult Edit(CandidateEditVM vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            var candidate = _context.Candidates.Find(vm.CandidateId);
            if (candidate == null) return NotFound();

            candidate.FullName = vm.FullName;
            candidate.Email = vm.Email;
            candidate.Phone = vm.Phone;
            candidate.DateOfBirth = vm.DateOfBirth;
            candidate.Status = vm.Status;

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
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var candidate = _context.Candidates.Find(id);
            if (candidate == null) return NotFound();

            _context.Candidates.Remove(candidate);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        // =====================
        private string HashPassword(string password)
        {
            byte[] salt = RandomNumberGenerator.GetBytes(16);

            return Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 32));
        }
    }
}
