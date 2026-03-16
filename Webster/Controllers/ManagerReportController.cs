using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Webster.Data;
using Webster.Models.ViewModels;

namespace Webster.Controllers
{
    public class ManagerReportController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ManagerReportController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string filter)
        {
            var query = _context.CandidateTestSections
                .Include(x => x.Candidate)
                .Include(x => x.TestSection)
                .Where(x => x.IsCompleted && x.CompletedAt != null)
                .AsQueryable();

            var today = DateTime.Today;

            if (filter == "today")
                query = query.Where(x => x.CompletedAt.Value.Date == today);

            if (filter == "week")
            {
                var start = today.AddDays(-(int)today.DayOfWeek);
                query = query.Where(x => x.CompletedAt >= start);
            }

            if (filter == "month")
                query = query.Where(x => x.CompletedAt.Value.Month == today.Month);

            // ================================
            // GROUP BY CANDIDATE (TOTAL SCORE)
            // ================================

            var data = await query
                .GroupBy(x => new { x.CandidateId, x.Candidate.FullName })
                .Select(g => new
                {
                    CandidateId = g.Key.CandidateId,
                    CandidateName = g.Key.FullName,
                    TotalScore = g.Sum(x => x.Score),
                    Passed = g.All(x => x.IsPassed),
                    Date = g.Max(x => x.CompletedAt)
                })
                .ToListAsync();

            var vm = new ReportDashboardVM();

            // ================================
            // DASHBOARD STATS
            // ================================

            vm.TotalTests = data.Count;

            vm.Passed = data.Count(x => x.Passed);

            vm.Failed = data.Count(x => !x.Passed);

            vm.AverageScore = data.Count == 0
                ? 0
                : data.Average(x => x.TotalScore);

            // ================================
            // RESULTS TABLE
            // ================================

            vm.Results = data
                .Select(x => new ResultRowVM
                {
                    CandidateName = x.CandidateName,
                    Section = "Final Test",
                    Score = x.TotalScore,
                    Passed = x.Passed,
                    Date = x.Date!.Value
                })
                .OrderByDescending(x => x.Date)
                .ToList();

            // ================================
            // TOP CANDIDATES
            // ================================

            vm.TopCandidates = data
                .OrderByDescending(x => x.TotalScore)
                .Take(5)
                .Select(x => new TopCandidateVM
                {
                    Name = x.CandidateName,
                    Score = x.TotalScore
                })
                .ToList();

            // ================================
            // CHART DATA (TESTS PER DAY)
            // ================================

            var chart = data
                .GroupBy(x => x.Date!.Value.Date)
                .Select(g => new
                {
                    Date = g.Key,
                    Count = g.Count()
                })
                .OrderBy(x => x.Date)
                .ToList();

            vm.TestDates = chart
                .Select(x => x.Date.ToString("dd/MM"))
                .ToList();

            vm.TestCounts = chart
                .Select(x => x.Count)
                .ToList();

            return View(vm);
        }

        // =====================================
        // EXPORT EXCEL (TOTAL SCORE PER CANDIDATE)
        // =====================================

        public async Task<IActionResult> ExportExcel()
        {
            var data = await _context.CandidateTestSections
                .Include(x => x.Candidate)
                .Where(x => x.IsCompleted)
                .GroupBy(x => new { x.CandidateId, x.Candidate.FullName })
                .Select(g => new
                {
                    CandidateName = g.Key.FullName,
                    TotalScore = g.Sum(x => x.Score),
                    Passed = g.All(x => x.IsPassed),
                    Date = g.Max(x => x.CompletedAt)
                })
                .ToListAsync();

            using var workbook = new XLWorkbook();
            var ws = workbook.Worksheets.Add("Report");

            ws.Cell(1, 1).Value = "Candidate";
            ws.Cell(1, 2).Value = "Total Score";
            ws.Cell(1, 3).Value = "Result";
            ws.Cell(1, 4).Value = "Completed Date";

            int row = 2;

            foreach (var r in data)
            {
                ws.Cell(row, 1).Value = r.CandidateName;
                ws.Cell(row, 2).Value = r.TotalScore;
                ws.Cell(row, 3).Value = r.Passed ? "Pass" : "Fail";
                ws.Cell(row, 4).Value = r.Date;

                row++;
            }

            ws.Columns().AdjustToContents();

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);

            return File(
                stream.ToArray(),
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "CandidateReport.xlsx"
            );
        }
    }
}