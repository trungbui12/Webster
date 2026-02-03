using System.ComponentModel.DataAnnotations;
using Webster.Models.Enums;

namespace Webster.Models.ViewModels
{
    public class TestSectionCreateVM
    {
        [Required]
        public TestSectionType SectionType { get; set; }

        [Required]
        [Range(1, 300)]
        public int DurationInMinutes { get; set; }

        [Required]
        [Range(1, 500)]
        public int TotalQuestions { get; set; }
    }
}
