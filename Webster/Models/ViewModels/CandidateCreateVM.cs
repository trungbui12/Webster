using System.ComponentModel.DataAnnotations;

namespace Webster.Models.ViewModels
{
    public class CandidateCreateVM
    {
        // ===== BASIC INFO =====
        [Required, MaxLength(100)]
        public string FullName { get; set; } = null!;

        [Required, EmailAddress]
        public string Email { get; set; } = null!;

        [Required, MaxLength(20)]
        public string Phone { get; set; } = null!;

        [Required]
        public DateTime DateOfBirth { get; set; }

        [Required]
        public string Username { get; set; } = null!;

        [Required, MinLength(6)]
        public string Password { get; set; } = null!;

        // ===== EDUCATION =====
        [Required]
        public string Degree { get; set; } = null!;

        [Required]
        public string University { get; set; } = null!;

        [Required]
        public int GraduationYear { get; set; }

        // ===== EXPERIENCE =====
        [Required]
        public int YearsOfExperience { get; set; }

        public string? PreviousCompany { get; set; }
    }
}