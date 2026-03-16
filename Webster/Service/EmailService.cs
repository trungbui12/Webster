using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;
using Webster.Models.Settings;

namespace Webster.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _settings;

        public EmailService(IOptions<EmailSettings> settings)
        {
            _settings = settings.Value;
        }

        public async Task SendCandidateAccountAsync(string toEmail, string username, string password)
        {
            var smtp = new SmtpClient(_settings.Host, _settings.Port)
            {
                Credentials = new NetworkCredential(_settings.SenderEmail, _settings.Password),
                EnableSsl = true
            };

            var mail = new MailMessage
            {
                From = new MailAddress(_settings.SenderEmail, _settings.SenderName),
                Subject = "Your Webster Test Account",
                IsBodyHtml = true,
                Body = $@"
                    <div style='font-family:Arial;padding:20px'>
                        <h2>Webster Aptitude Test Invitation</h2>

                        <p>Your candidate account has been created.</p>

                        <div style='background:#f4f4f4;padding:15px;border-radius:8px'>
                            <p><b>Username:</b> {username}</p>
                            <p><b>Password:</b> {password}</p>
                        </div>

                        <p style='margin-top:20px'>
                            Login here:
                        </p>

                        <a href='https://yourdomain.com/candidate/login'
                        style='background:#2563eb;color:white;padding:10px 18px;border-radius:6px;text-decoration:none'>
                        Start Test
                        </a>

                        <p style='margin-top:20px;font-size:12px;color:gray'>
                            Webster Recruitment System
                        </p>
                    </div>"
            };

            mail.To.Add(toEmail);

            await smtp.SendMailAsync(mail);
        }
    }
}