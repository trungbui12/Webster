using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Webster.Models.Entities
{
    public class TestResult
    {
        [Key]
        public int TestResultId { get; set; }

        [Required]
        public int CandidateId { get; set; }

        public int TotalScore { get; set; }

        public bool IsPassed { get; set; }

        public DateTime CompletedAt { get; set; }

        [ForeignKey(nameof(CandidateId))]
        public Candidate Candidate { get; set; } = null!;
    }
}
