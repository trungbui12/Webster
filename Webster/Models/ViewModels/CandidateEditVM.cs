using System;
using System.ComponentModel.DataAnnotations;
using Webster.Models.Enums;

namespace Webster.Models.ViewModels
{
    public class CandidateEditVM
    {
        public int CandidateId { get; set; }

        // ===== BASIC =====
        [Required]
        public string FullName { get; set; } = null!;

        [Required, EmailAddress]
        public string Email { get; set; } = null!;

        public string? Phone { get; set; }

        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        public CandidateStatus Status { get; set; }

        // ===== EDUCATION =====
        public string? Degree { get; set; }
        public string? University { get; set; }
        [Required]
        public int GraduationYear { get; set; }

        // ===== EXPERIENCE =====
        [Required]
        public int YearsOfExperience { get; set; }
        public string? PreviousCompany { get; set; }
    }
}