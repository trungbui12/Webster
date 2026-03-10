using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Webster.Models.Entities
{
    public class CandidateAnswer
    {
        [Key]
        public int CandidateAnswerId { get; set; }

        [Required]
        public int CandidateId { get; set; }

        [Required]
        public int QuestionId { get; set; }

        // Cho Single / Multiple
        public int? AnswerId { get; set; }

        // Cho Text answer
        public string? TextAnswer { get; set; }

        public DateTime AnsweredAt { get; set; }

        [ForeignKey(nameof(CandidateId))]
        public Candidate Candidate { get; set; } = null!;

        [ForeignKey(nameof(QuestionId))]
        public Question Question { get; set; } = null!;

        [ForeignKey(nameof(AnswerId))]
        public Answer? Answer { get; set; }
    }
}
