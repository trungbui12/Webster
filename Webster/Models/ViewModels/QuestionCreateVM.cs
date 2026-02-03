using System.ComponentModel.DataAnnotations;
using Webster.Models.Enums;

namespace Webster.Models.ViewModels
{
    public class QuestionCreateVM
    {
        [Required]
        public int TestSectionId { get; set; }

        [Required]
        public string Content { get; set; } = null!;

        [Range(1, 5)]
        public int Score { get; set; }

        public QuestionDifficulty Difficulty { get; set; }

        // 🔥 đáp án đúng
        [Required]
        public int CorrectAnswerId { get; set; }

        public List<AnswerCreateVM> Answers { get; set; } = new();
    }
}
