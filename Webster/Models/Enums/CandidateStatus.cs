namespace Webster.Models.Enums
{
    public enum CandidateStatus
    {
        Created = 0,

        InGeneralKnowledge = 1,
        CompletedGeneralKnowledge = 2,

        InMathematics = 3,
        CompletedMathematics = 4,

        InComputerTechnology = 5,
        CompletedTest = 6,

        Passed = 7,
        Failed = 8,
        InProgress = 9
    }
}
