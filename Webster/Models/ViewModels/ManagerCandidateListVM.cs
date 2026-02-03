namespace Webster.Models.ViewModels
{
    public class ManagerCandidateListVM
    {
        public int CandidateId { get; set; }
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Status { get; set; } = null!;

        public int CompletedSections { get; set; }
        public int TotalSections { get; set; }

        public int? TotalScore { get; set; }
        public bool? IsPassed { get; set; }
        public DateTime? CompletedAt { get; set; }
    }
}
