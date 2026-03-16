using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Webster.Data;
using Webster.Models.Entities;

namespace Webster.Controllers
{
    [Authorize(Roles = "HR")]
    public class HRController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HRController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ================= DASHBOARD + PASSED CANDIDATES =================
        public async Task<IActionResult> Dashboard(string search, int page = 1)
        {
            int pageSize = 10;

            // statistics
            int totalCandidates = await _context.Candidates.CountAsync();
            int passedCandidates = await _context.PassedCandidates.CountAsync();

            ViewBag.TotalCandidates = totalCandidates;
            ViewBag.PassedCandidates = passedCandidates;
            ViewBag.FailedCandidates = totalCandidates - passedCandidates;

            // query passed candidates
            var query = _context.PassedCandidates
                .Include(x => x.Candidate)
                .ThenInclude(c => c.TestResult)
                .AsQueryable();

            // search
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(x =>
                    x.Candidate.FullName.Contains(search) ||
                    x.Candidate.Email.Contains(search) ||
                    x.Candidate.Phone.Contains(search));
            }

            int totalItems = await query.CountAsync();

            var data = await query
                .OrderByDescending(x => x.PassedDate)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            ViewBag.TotalItems = totalItems;
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalItems / pageSize);
            ViewBag.Search = search;

            return View(data);
        }

        // ================= VIEW CANDIDATE PROFILE =================
        public async Task<IActionResult> ViewProfile(int id)
        {
            var candidate = await _context.Candidates
                .Include(x => x.Education)
                .Include(x => x.Experience)
                .Include(x => x.TestResult)
                .FirstOrDefaultAsync(x => x.CandidateId == id);

            if (candidate == null)
                return NotFound();

            return View(candidate);
        }
    }
}