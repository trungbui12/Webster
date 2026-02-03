using System.ComponentModel.DataAnnotations;

namespace Webster.Models.ViewModels
{
    public class AnswerEditVM
    {
        public int AnswerId { get; set; }
        public string Content { get; set; } = null!;
        public bool IsCorrect { get; set; }
    }
}
