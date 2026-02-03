using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Webster.Models.Entities
{
    public class Experience
    {
        [Key]
        public int ExperienceId { get; set; }

        [Required]
        public int CandidateId { get; set; }

        [Required]
        public int YearsOfExperience { get; set; }

        [MaxLength(200)]
        public string? PreviousCompany { get; set; }

        [ForeignKey(nameof(CandidateId))]
        public Candidate Candidate { get; set; } = null!;
    }
}
