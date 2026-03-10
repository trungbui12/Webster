using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Webster.Data;
using Webster.Models.ViewModels;

namespace Webster.Controllers
{
    public class ManagerCandidateController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ManagerCandidateController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(string? search,
                           string? sortOrder,
                           DateTime? fromDate,
                           DateTime? toDate,
                           int page = 1)
        {
            int pageSize = 5;

            var query = _context.Candidates
                .Include(c => c.CandidateTestSections)
                .Include(c => c.TestResult)
                .AsQueryable();

            // SEARCH
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(c =>
                    c.FullName.Contains(search) ||
                    c.Email.Contains(search));
            }

            // FILTER DATE
            if (fromDate.HasValue)
                query = query.Where(c =>
                    c.TestResult != null &&
                    c.TestResult.CompletedAt >= fromDate);

            if (toDate.HasValue)
                query = query.Where(c =>
                    c.TestResult != null &&
                    c.TestResult.CompletedAt <= toDate);

            // SORT
            query = sortOrder switch
            {
                "score_desc" => query.OrderByDescending(c => c.TestResult!.TotalScore),
                "score_asc" => query.OrderBy(c => c.TestResult!.TotalScore),
                _ => query.OrderByDescending(c => c.TestResult!.CompletedAt)
            };

            int totalSections = _context.TestSections.Count();

            var data = query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(c => new ManagerCandidateListVM
                {
                    CandidateId = c.CandidateId,
                    FullName = c.FullName,
                    Email = c.Email,
                    CompletedSections = c.CandidateTestSections.Count(x => x.IsCompleted),
                    TotalSections = totalSections,
                    TotalScore = c.TestResult != null ? c.TestResult.TotalScore : null,
                    IsPassed = c.TestResult != null ? c.TestResult.IsPassed : null,
                    CompletedAt = c.TestResult != null ? c.TestResult.CompletedAt : null
                })
                .ToList();

            // 🔥 QUAN TRỌNG NHẤT
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_CandidateList", data);
            }

            return View(data);
        }
        public IActionResult ExportExcel()
        {
            var data = _context.Candidates
                .Include(c => c.TestResult)
                .ToList();

            using var workbook = new XLWorkbook();
            var ws = workbook.Worksheets.Add("Candidates");

            ws.Cell(1, 1).Value = "Full Name";
            ws.Cell(1, 2).Value = "Email";
            ws.Cell(1, 3).Value = "Score";
            ws.Cell(1, 4).Value = "Result";

            for (int i = 0; i < data.Count; i++)
            {
                var c = data[i];
                ws.Cell(i + 2, 1).Value = c.FullName;
                ws.Cell(i + 2, 2).Value = c.Email;
                ws.Cell(i + 2, 3).Value = c.TestResult?.TotalScore;
                ws.Cell(i + 2, 4).Value =
                    c.TestResult?.IsPassed == true ? "Passed" :
                    c.TestResult?.IsPassed == false ? "Failed" :
                    "In Progress";
            }

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);

            return File(stream.ToArray(),
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "Candidates.xlsx");
        }
    }
}