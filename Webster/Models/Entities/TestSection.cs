using System.ComponentModel.DataAnnotations;
using Webster.Models.Enums;

namespace Webster.Models.Entities
{
    public class TestSection
    {
        [Key]
        public int TestSectionId { get; set; }

        [Required]
        public TestSectionType SectionType { get; set; }

        [Required]
        public int DurationInMinutes { get; set; }

        [Required]
        public int TotalQuestions { get; set; }

        public ICollection<Question> Questions { get; set; } = new List<Question>();
        public ICollection<CandidateTestSection> CandidateTestSections { get; set; } = new List<CandidateTestSection>();

    }
}
