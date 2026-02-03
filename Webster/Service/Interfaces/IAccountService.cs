using Webster.Models.Entities;

namespace Webster.Services.Interfaces
{
    public interface IAccountService
    {
        Manager? LoginManager(string username, string password);
        Candidate? LoginCandidate(string username, string password);
    }
}
