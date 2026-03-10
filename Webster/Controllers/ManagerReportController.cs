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

            var data = await query.ToListAsync();

            var vm = new ReportDashboardVM();

            vm.TotalTests = data.Count;
            vm.Passed = data.Count(x => x.IsPassed);
            vm.Failed = data.Count(x => !x.IsPassed);
            vm.AverageScore = data.Count == 0 ? 0 : data.Average(x => x.Score);

            vm.Results = data.Select(x => new ResultRowVM
            {
                CandidateName = x.Candidate.FullName,
                Section = x.TestSection.SectionType.ToString(),
                Score = x.Score,
                Passed = x.IsPassed,
                Date = x.CompletedAt!.Value
            }).OrderByDescending(x => x.Date).ToList();

            vm.TopCandidates = data
                .OrderByDescending(x => x.Score)
                .Take(5)
                .Select(x => new TopCandidateVM
                {
                    Name = x.Candidate.FullName,
                    Score = x.Score
                }).ToList();

            var chart = data
                .GroupBy(x => x.CompletedAt!.Value.Date)
                .Select(g => new
                {
                    Date = g.Key,
                    Count = g.Count()
                })
                .OrderBy(x => x.Date)
                .ToList();

            vm.TestDates = chart.Select(x => x.Date.ToString("dd/MM")).ToList();
            vm.TestCounts = chart.Select(x => x.Count).ToList();

            return View(vm);
        }

        public async Task<IActionResult> ExportExcel()
        {
            var data = await _context.CandidateTestSections
                .Include(x => x.Candidate)
                .Include(x => x.TestSection)
                .Where(x => x.IsCompleted)
                .ToListAsync();

            using var workbook = new XLWorkbook();
            var ws = workbook.Worksheets.Add("Report");

            ws.Cell(1, 1).Value = "Candidate";
            ws.Cell(1, 2).Value = "Section";
            ws.Cell(1, 3).Value = "Score";
            ws.Cell(1, 4).Value = "Result";
            ws.Cell(1, 5).Value = "Date";

            int row = 2;

            foreach (var r in data)
            {
                ws.Cell(row, 1).Value = r.Candidate.FullName;
                ws.Cell(row, 2).Value = r.TestSection.SectionType.ToString();
                ws.Cell(row, 3).Value = r.Score;
                ws.Cell(row, 4).Value = r.IsPassed ? "Pass" : "Fail";
                ws.Cell(row, 5).Value = r.CompletedAt;

                row++;
            }

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);

            return File(stream.ToArray(),
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "Report.xlsx");
        }
    }
}