using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Webster.Models.Entities
{
    public class Education
    {
        [Key]
        public int EducationId { get; set; }

        [Required]
        public int CandidateId { get; set; }

        [Required, MaxLength(100)]
        public string Degree { get; set; } = null!;

        [Required, MaxLength(100)]
        public string University { get; set; } = null!;

        [Required]
        public int GraduationYear { get; set; }

        [ForeignKey(nameof(CandidateId))]
        public Candidate Candidate { get; set; } = null!;
    }
}
