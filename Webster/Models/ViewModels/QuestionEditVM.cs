using System.ComponentModel.DataAnnotations;
using Webster.Models.Enums;

namespace Webster.Models.ViewModels
{
    public class QuestionEditVM
    {
        public int QuestionId { get; set; }

        [Required]
        public int TestSectionId { get; set; }

        [Required]
        public string Content { get; set; } = null!;

        [Range(1, 5)]
        public int Score { get; set; }

        public QuestionDifficulty Difficulty { get; set; }

        // 🔥 ID của đáp án đúng
        [Required]
        public int CorrectAnswerId { get; set; }

        public List<AnswerEditVM> Answers { get; set; } = new();
    }
}
