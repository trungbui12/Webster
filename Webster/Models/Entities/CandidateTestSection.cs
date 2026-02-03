using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Webster.Models.Entities
{
    public class CandidateTestSection
    {
        [Key]
        public int CandidateTestSectionId { get; set; }

        // ================= FK =================
        [Required]
        public int CandidateId { get; set; }

        [Required]
        public int TestSectionId { get; set; }

        // ================= TEST STATE =================
        public bool IsStarted { get; set; } = false;

        public DateTime? StartedAt { get; set; }

        public bool IsCompleted { get; set; } = false;

        public DateTime? CompletedAt { get; set; }

        // ================= RESULT =================
        public int Score { get; set; } = 0;

        public bool IsPassed { get; set; } = false;

        // ================= NAVIGATION =================
        [ForeignKey(nameof(CandidateId))]
        public Candidate Candidate { get; set; } = null!;

        [ForeignKey(nameof(TestSectionId))]
        public TestSection TestSection { get; set; } = null!;
    }
}
