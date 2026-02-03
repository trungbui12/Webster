using System.ComponentModel.DataAnnotations;
using Webster.Models.Enums;

namespace Webster.Models.Entities
{
    public class Candidate
    {
        [Key]
        public int CandidateId { get; set; }

        [Required, MaxLength(100)]
        public string FullName { get; set; } = null!;

        [Required, EmailAddress]
        public string Email { get; set; } = null!;

        [Required, MaxLength(20)]
        public string Phone { get; set; } = null!;

        [Required]
        public DateTime DateOfBirth { get; set; }

        [Required]
        public CandidateStatus Status { get; set; }

        [Required, MaxLength(50)]
        public string Username { get; set; } = null!;

        [Required]
        public string PasswordHash { get; set; } = null!;

        public Education? Education { get; set; }
        public Experience? Experience { get; set; }

        public ICollection<CandidateAnswer> CandidateAnswers { get; set; } = new List<CandidateAnswer>();
        public ICollection<CandidateTestSection> CandidateTestSections { get; set; } = new List<CandidateTestSection>();

        public TestResult? TestResult { get; set; }

    }
}
