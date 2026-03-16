using System;
using System.ComponentModel.DataAnnotations;
using Webster.Models.Enums;

namespace Webster.Models.ViewModels
{
    public class CandidateEditVM
    {
        public int CandidateId { get; set; }

        // ===== BASIC =====

        [Required(ErrorMessage = "Full name is required")]
        [StringLength(100, ErrorMessage = "Full name cannot exceed 100 characters")]
        public string FullName { get; set; } = null!;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address")]
        public string Email { get; set; } = null!;

        [RegularExpression(@"^0\d{9,10}$", ErrorMessage = "Invalid phone number")]
        public string? Phone { get; set; }

        [Required(ErrorMessage = "Date of birth is required")]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        public CandidateStatus Status { get; set; }

        // ===== EDUCATION =====

        [StringLength(100)]
        public string? Degree { get; set; }

        [StringLength(150)]
        public string? University { get; set; }

        [Required(ErrorMessage = "Graduation year is required")]
        [Range(1950, 2100, ErrorMessage = "Invalid graduation year")]
        public int GraduationYear { get; set; }

        // ===== EXPERIENCE =====

        [Required(ErrorMessage = "Years of experience is required")]
        [Range(0, 50, ErrorMessage = "Experience must be between 0 and 50 years")]
        public int YearsOfExperience { get; set; }

        [StringLength(150)]
        public string? PreviousCompany { get; set; }
    }
}