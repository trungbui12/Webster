using System.Threading.Tasks;

namespace Webster.Services
{
    public interface IEmailService
    {
        Task SendCandidateAccountAsync(string toEmail, string username, string password);
    }
}