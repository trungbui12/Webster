using System.ComponentModel.DataAnnotations;

namespace Webster.Models.ViewModels
{
    public class CandidateCreateVM
    {
        // ===== BASIC INFO =====

        [Required(ErrorMessage = "Full name is required")]
        [StringLength(100, ErrorMessage = "Full name cannot exceed 100 characters")]
        public string FullName { get; set; } = null!;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Phone number is required")]
        [RegularExpression(@"^0\d{9,10}$", ErrorMessage = "Phone must be a valid Vietnamese number")]
        public string Phone { get; set; } = null!;

        [Required(ErrorMessage = "Date of birth is required")]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        // ===== ACCOUNT =====

        [Required(ErrorMessage = "Username is required")]
        [StringLength(50, ErrorMessage = "Username cannot exceed 50 characters")]
        public string Username { get; set; } = null!;

        [Required(ErrorMessage = "Password is required")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;

        // ===== EDUCATION =====

        [Required(ErrorMessage = "Degree is required")]
        [StringLength(100)]
        public string Degree { get; set; } = null!;

        [Required(ErrorMessage = "University is required")]
        [StringLength(150)]
        public string University { get; set; } = null!;

        [Range(1950, 2100, ErrorMessage = "Graduation year must be valid")]
        public int GraduationYear { get; set; }

        // ===== EXPERIENCE =====

        [Range(0, 50, ErrorMessage = "Experience must be between 0 and 50 years")]
        public int YearsOfExperience { get; set; }

        [StringLength(150)]
        public string? PreviousCompany { get; set; }
    }
}