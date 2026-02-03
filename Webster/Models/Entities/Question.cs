using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Webster.Models.Enums;

namespace Webster.Models.Entities
{
    public class Question
    {
        [Key]
        public int QuestionId { get; set; }

        [Required]
        public int TestSectionId { get; set; }

        [Required]
        public string Content { get; set; } = null!;

        [Required, Range(1, 5)]
        public int Score { get; set; }

        [Required]
        public QuestionDifficulty Difficulty { get; set; }

        [ForeignKey(nameof(TestSectionId))]
        public TestSection TestSection { get; set; } = null!;

        public ICollection<Answer> Answers { get; set; } = new List<Answer>();
    }
}
