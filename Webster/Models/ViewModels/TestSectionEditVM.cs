using System.ComponentModel.DataAnnotations;
using Webster.Models.Enums;

namespace Webster.Models.ViewModels
{
    public class TestSectionEditVM
    {
        public int TestSectionId { get; set; }

        [Required]
        public TestSectionType SectionType { get; set; }

        [Required]
        public int DurationInMinutes { get; set; }

        [Required]
        public int TotalQuestions { get; set; }
    }
}
