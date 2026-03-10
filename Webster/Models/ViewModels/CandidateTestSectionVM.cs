using Webster.Models.Enums;

namespace Webster.Models.ViewModels
{
    public class CandidateTestSectionVM
    {
        public int TestSectionId { get; set; }

        public TestSectionType SectionType { get; set; }

        public bool IsStarted { get; set; }

        public bool IsCompleted { get; set; }

        public int Score { get; set; }

        public DateTime? StartedAt { get; set; }

        public DateTime? CompletedAt { get; set; }


        // NEW - dùng cho Dashboard UI

        public int QuestionCount { get; set; }

        public int TimeLimit { get; set; }   // minutes
    }
}