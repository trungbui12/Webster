using System.ComponentModel.DataAnnotations;
using Webster.Models.Enums;

namespace Webster.Models.ViewModels
{
    public class QuestionCreateVM
    {
        [Required]
        public int TestSectionId { get; set; }

        [Required]
        [StringLength(1000)]
        public string Content { get; set; } = null!;

        [Range(1, 5)]
        public int Score { get; set; }

        [Required]
        public QuestionDifficulty Difficulty { get; set; }

        [Required]
        public QuestionType QuestionType { get; set; }

        // ===== SINGLE =====
        public int? CorrectAnswerId { get; set; }

        // ===== MULTIPLE =====
        public List<int> CorrectAnswerIds { get; set; } = new();

        // ===== TEXT =====
        [StringLength(500)]
        public string? CorrectTextAnswer { get; set; }

        // ===== ANSWER LIST (cho Single, Multiple, TrueFalse) =====
        public List<AnswerCreateVM> Answers { get; set; } = new();
    }
}