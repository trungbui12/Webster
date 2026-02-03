using Webster.Data;
using Webster.Helpers;
using Webster.Models.Entities;
using Webster.Services.Interfaces;

namespace Webster.Services.Implementations
{
    public class AccountService : IAccountService
    {
        private readonly ApplicationDbContext _context;

        public AccountService(ApplicationDbContext context)
        {
            _context = context;
        }

        public Manager? LoginManager(string username, string password)
        {
            var manager = _context.Managers
                .FirstOrDefault(m => m.Username == username);

            if (manager == null) return null;

            return PasswordHasher.Verify(password, manager.PasswordHash)
                ? manager
                : null;
        }

        public Candidate? LoginCandidate(string username, string password)
        {
            var candidate = _context.Candidates
                .FirstOrDefault(c => c.Username == username);

            if (candidate == null) return null;

            return PasswordHasher.Verify(password, candidate.PasswordHash)
                ? candidate
                : null;
        }
    }
}
