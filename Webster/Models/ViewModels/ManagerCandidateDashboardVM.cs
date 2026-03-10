namespace Webster.Models.ViewModels
{
    public class ManagerCandidateDashboardVM
    {
        public List<ManagerCandidateListVM> Candidates { get; set; } = new();

        public int TotalCandidates { get; set; }
        public int TotalPassed { get; set; }
        public double PassRate { get; set; }

        public string? Search { get; set; }
        public string? Sort { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }

        public int Page { get; set; }
        public int TotalPages { get; set; }
    }
}
