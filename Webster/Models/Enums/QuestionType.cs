using System.ComponentModel.DataAnnotations;

namespace Webster.Models.Enums
{
    public enum QuestionType
    {
        [Display(Name = "Single Choice (1 correct)")]
        SingleChoice = 1,

        [Display(Name = "Multiple Choice (multiple correct)")]
        MultipleChoice = 2,

        [Display(Name = "True / False")]
        TrueFalse = 3,

        [Display(Name = "Text Answer")]
        Text = 4
    }
}
