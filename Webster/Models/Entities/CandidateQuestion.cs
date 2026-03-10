using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Webster.Models.Entities
{
    public class CandidateQuestion
    {
        [Key]
        public int CandidateQuestionId { get; set; }

        // ================= Candidate =================
        [Required]
        public int CandidateId { get; set; }

        [ForeignKey(nameof(CandidateId))]
        public Candidate Candidate { get; set; }


        // ================= Test Section =================
        [Required]
        public int TestSectionId { get; set; }

        [ForeignKey(nameof(TestSectionId))]
        public TestSection TestSection { get; set; }


        // ================= Question =================
        [Required]
        public int QuestionId { get; set; }

        [ForeignKey(nameof(QuestionId))]
        public Question Question { get; set; }


        // ================= Question Order =================
        // giữ thứ tự câu hỏi khi load lại
        public int OrderIndex { get; set; }


        // ================= Metadata =================
        public DateTime AssignedAt { get; set; } = DateTime.Now;
    }
}