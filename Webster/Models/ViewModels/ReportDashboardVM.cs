namespace Webster.Models.ViewModels
{
    public class ReportDashboardVM
    {
        public int TotalTests { get; set; }

        public int Passed { get; set; }

        public int Failed { get; set; }

        public double AverageScore { get; set; }

        public List<string> TestDates { get; set; } = new();

        public List<int> TestCounts { get; set; } = new();

        public List<TopCandidateVM> TopCandidates { get; set; } = new();

        public List<ResultRowVM> Results { get; set; } = new();
    }

    public class TopCandidateVM
    {
        public string Name { get; set; } = "";

        public int Score { get; set; }
    }

    public class ResultRowVM
    {
        public string CandidateName { get; set; } = "";

        public string Section { get; set; } = "";

        public int Score { get; set; }

        public bool Passed { get; set; }

        public DateTime Date { get; set; }
    }
}