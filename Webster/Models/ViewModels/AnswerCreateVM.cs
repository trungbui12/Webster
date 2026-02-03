using System.ComponentModel.DataAnnotations;

namespace Webster.Models.ViewModels
{
    public class AnswerCreateVM
    {
        [Required]
        public int QuestionId { get; set; }

        [Required]
        public string Content { get; set; } = null!;

        public bool IsCorrect { get; set; }
    }
}
