using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Webster.Models.Entities
{
    public class PassedCandidate
    {
        [Key]
        public int PassedCandidateId { get; set; }

        [Required]
        public int CandidateId { get; set; }

        public DateTime PassedDate { get; set; }

        [ForeignKey(nameof(CandidateId))]
        public Candidate Candidate { get; set; } = null!;
    }
}
