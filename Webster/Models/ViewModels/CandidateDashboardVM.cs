using Webster.Models.Entities;
using Webster.Models.Enums;

namespace Webster.Models.ViewModels
{
    public class CandidateDashboardVM
    {
        public int CandidateId { get; set; }
        public string FullName { get; set; } = null!;
        public CandidateStatus Status { get; set; }

        public List<CandidateTestSectionVM> TestSections { get; set; } = new();

        public int? TotalScore { get; set; }
        public bool? IsPassed { get; set; }
        public int QuestionCount { get; set; }

        public int TimeLimit { get; set; } // minutes
    }
}
