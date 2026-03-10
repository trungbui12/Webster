using Webster.Models.Enums;

namespace Webster.Models.ViewModels
{
    public class QuestionVM
    {
        public int QuestionId { get; set; }

        public string Content { get; set; } = null!;

        public int Score { get; set; }

        public QuestionType QuestionType { get; set; }

        public List<AnswerVM> Answers { get; set; } = new();

        // Single
        public int? SelectedAnswerId { get; set; }

        // Multiple
        public List<int> SelectedAnswerIds { get; set; } = new();

        // Text
        public string? TextAnswer { get; set; }
    }
}