using Webster.Helpers;
using Webster.Models.Entities;
using Webster.Models.Enums;

namespace Webster.Data
{
    public static class DbInitializer
    {
        public static void Seed(ApplicationDbContext context)
        {
            context.Database.EnsureCreated();

            // ================= MANAGER =================
            if (!context.Managers.Any())
            {
                var manager = new Manager
                {
                    FullName = "System Manager",
                    Username = "admin",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("123456")
                };

                context.Managers.Add(manager);
            }
            // ================= HR =================
            if (!context.HRs.Any())
            {
                var hr = new HR
                {
                    FullName = "HR Manager",
                    Username = "hr",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("123456")
                };

                context.HRs.Add(hr);
            }

            // ================= CANDIDATE =================
            if (!context.Candidates.Any())
            {
                var candidate = new Candidate
                {
                    FullName = "Test Candidate",
                    Email = "candidate@test.com",
                    Phone = "0123456789",
                    DateOfBirth = new DateTime(2000, 1, 1),
                    Username = "candidate",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("123456"),
                    Status = CandidateStatus.Created
                };

                context.Candidates.Add(candidate);
            }

            context.SaveChanges();
        }
    }
}
