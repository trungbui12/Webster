namespace Webster.Models.ViewModels
{
    public class SaveAnswerVM
    {
        public int QuestionId { get; set; }

        public int? AnswerId { get; set; }

        public string? TextAnswer { get; set; }
    }
}
