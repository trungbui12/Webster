using Webster.Models.Enums;

namespace Webster.Models.ViewModels
{
    public class CandidateTestVM
    {
        public int TestSectionId { get; set; }
        public TestSectionType SectionType { get; set; }
        public int DurationInMinutes { get; set; }

        public List<QuestionVM> Questions { get; set; } = new();
    }

    public class QuestionVM
    {
        public int QuestionId { get; set; }
        public string Content { get; set; } = null!;
        public int Score { get; set; }

        public List<AnswerVM> Answers { get; set; } = new();

        public int? SelectedAnswerId { get; set; }
    }

    public class AnswerVM
    {
        public int AnswerId { get; set; }
        public string Content { get; set; } = null!;
    }
}
