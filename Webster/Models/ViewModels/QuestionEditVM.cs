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
        [StringLength(1000)]
        public string Content { get; set; } = null!;

        [Range(1, 5)]
        public int Score { get; set; }

        [Required]
        public QuestionDifficulty Difficulty { get; set; }

        // 🔥 THÊM QuestionType (bắt buộc để Edit giống Create)
        [Required]
        public QuestionType QuestionType { get; set; }

        // ===== SINGLE =====
        public int? CorrectAnswerId { get; set; }

        // ===== MULTIPLE =====
        public List<int> CorrectAnswerIds { get; set; } = new();

        // ===== TEXT =====
        [StringLength(500)]
        public string? CorrectTextAnswer { get; set; }

        // ===== ANSWERS =====
        public List<AnswerEditVM> Answers { get; set; } = new();
    }
}