using Microsoft.EntityFrameworkCore;
using Webster.Models.Entities;

namespace Webster.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // ===== DbSet =====
        public DbSet<Candidate> Candidates => Set<Candidate>();
        public DbSet<Education> Educations => Set<Education>();
        public DbSet<Experience> Experiences => Set<Experience>();
        public DbSet<Manager> Managers => Set<Manager>();

        public DbSet<TestSection> TestSections => Set<TestSection>();
        public DbSet<Question> Questions => Set<Question>();
        public DbSet<Answer> Answers => Set<Answer>();

        public DbSet<CandidateAnswer> CandidateAnswers => Set<CandidateAnswer>();
        public DbSet<TestResult> TestResults => Set<TestResult>();
        public DbSet<PassedCandidate> PassedCandidates => Set<PassedCandidate>();
        public DbSet<CandidateTestSection> CandidateTestSections => Set<CandidateTestSection>();
        public DbSet<CandidateQuestion> CandidateQuestions { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ===================== Candidate =====================
            modelBuilder.Entity<Candidate>()
                .HasIndex(c => c.Username)
                .IsUnique();

            modelBuilder.Entity<Candidate>()
                .HasIndex(c => c.Email)
                .IsUnique();

            // Candidate - Education (1 - 1)
            modelBuilder.Entity<Candidate>()
                .HasOne(c => c.Education)
                .WithOne(e => e.Candidate)
                .HasForeignKey<Education>(e => e.CandidateId)
                .OnDelete(DeleteBehavior.Cascade);

            // Candidate - Experience (1 - 1)
            modelBuilder.Entity<Candidate>()
                .HasOne(c => c.Experience)
                .WithOne(e => e.Candidate)
                .HasForeignKey<Experience>(e => e.CandidateId)
                .OnDelete(DeleteBehavior.Cascade);

            // Candidate - CandidateAnswers (1 - n)
            modelBuilder.Entity<Candidate>()
                .HasMany(c => c.CandidateAnswers)
                .WithOne(ca => ca.Candidate)
                .HasForeignKey(ca => ca.CandidateId)
                .OnDelete(DeleteBehavior.Cascade);

            // Candidate - TestResult (1 - 1)
            modelBuilder.Entity<Candidate>()
                .HasOne(c => c.TestResult)
                .WithOne(tr => tr.Candidate)
                .HasForeignKey<TestResult>(tr => tr.CandidateId)
                .OnDelete(DeleteBehavior.Cascade);
            // ===================== CandidateTestSection =====================

            // Candidate (1) - (n) CandidateTestSection
            modelBuilder.Entity<CandidateTestSection>()
                .HasOne(cts => cts.Candidate)
                .WithMany(c => c.CandidateTestSections)
                .HasForeignKey(cts => cts.CandidateId)
                .OnDelete(DeleteBehavior.Cascade);

            // TestSection (1) - (n) CandidateTestSection
            modelBuilder.Entity<CandidateTestSection>()
                .HasOne(cts => cts.TestSection)
                .WithMany(ts => ts.CandidateTestSections)
                .HasForeignKey(cts => cts.TestSectionId)
                .OnDelete(DeleteBehavior.Restrict);

            // ❗ Mỗi Candidate chỉ có 1 record cho mỗi TestSection
            modelBuilder.Entity<CandidateTestSection>()
                .HasIndex(cts => new { cts.CandidateId, cts.TestSectionId })
                .IsUnique();

            // ===================== Manager =====================
            modelBuilder.Entity<Manager>()
                .HasIndex(m => m.Username)
                .IsUnique();

            // ===================== TestSection =====================
            modelBuilder.Entity<TestSection>()
                .HasMany(ts => ts.Questions)
                .WithOne(q => q.TestSection)
                .HasForeignKey(q => q.TestSectionId)
                .OnDelete(DeleteBehavior.Cascade);

            // ===================== Question =====================
            modelBuilder.Entity<Question>()
                .Property(q => q.Score)
                .HasDefaultValue(1);

            modelBuilder.Entity<Question>()
                .HasMany(q => q.Answers)
                .WithOne(a => a.Question)
                .HasForeignKey(a => a.QuestionId)
                .OnDelete(DeleteBehavior.Cascade);

            // ===================== CandidateAnswer =====================
            modelBuilder.Entity<CandidateAnswer>()
                .HasOne(ca => ca.Question)
                .WithMany()
                .HasForeignKey(ca => ca.QuestionId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<CandidateAnswer>()
                .HasOne(ca => ca.Answer)
                .WithMany()
                .HasForeignKey(ca => ca.AnswerId)
                .OnDelete(DeleteBehavior.Restrict);

            // Không cho 1 Candidate trả lời 1 Question nhiều lần
            modelBuilder.Entity<CandidateAnswer>()
                .HasIndex(ca => new { ca.CandidateId, ca.QuestionId })
                .IsUnique();

            // ===================== PassedCandidate =====================
            modelBuilder.Entity<PassedCandidate>()
                .HasOne(pc => pc.Candidate)
                .WithMany()
                .HasForeignKey(pc => pc.CandidateId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<CandidateQuestion>()
    .HasIndex(x => new { x.CandidateId, x.TestSectionId, x.QuestionId })
    .IsUnique();

            modelBuilder.Entity<CandidateQuestion>()
      .HasOne(cq => cq.Candidate)
      .WithMany()
      .HasForeignKey(cq => cq.CandidateId)
      .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<CandidateQuestion>()
                .HasOne(cq => cq.TestSection)
                .WithMany()
                .HasForeignKey(cq => cq.TestSectionId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<CandidateQuestion>()
                .HasOne(cq => cq.Question)
                .WithMany()
                .HasForeignKey(cq => cq.QuestionId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
