using System.Net;
using System.Net.Mail;

namespace Webster.Helpers
{
    public class EmailHelper
    {
        public static void SendAccountEmail(string toEmail, string username, string password)
        {
            var fromEmail = "yuuichirou67@gmail.com";
            var fromPassword = "xniounmzznidjsny";

            var smtp = new SmtpClient("smtp.gmail.com", 587)
            {
                Credentials = new NetworkCredential(fromEmail, fromPassword),
                EnableSsl = true
            };

            var mail = new MailMessage
            {
                From = new MailAddress(fromEmail, "Webster Recruitment System"),
                Subject = "Your Webster Candidate Account",
                IsBodyHtml = true,

                Body = $@"
    <div style='font-family:Arial,Helvetica,sans-serif;background:#f4f6f9;padding:40px;'>

        <div style='max-width:600px;margin:auto;background:white;border-radius:10px;overflow:hidden;box-shadow:0 5px 15px rgba(0,0,0,0.1)'>

            <!-- HEADER -->
            <div style='background:#2563eb;color:white;padding:25px;text-align:center'>
                <h2 style='margin:0'>Webster Recruitment System</h2>
                <p style='margin:5px 0 0 0;font-size:14px'>
                    Aptitude Test Candidate Account
                </p>
            </div>

            <!-- BODY -->
            <div style='padding:30px'>

                <p>Hello <b>{username}</b>,</p>

                <p>
                    Your candidate account has been successfully created in the 
                    <b>Webster Aptitude Test System</b>.
                </p>

                <p>
                    Please use the following credentials to login and start your test.
                </p>

                <!-- ACCOUNT BOX -->
                <div style='background:#f1f5f9;border-radius:8px;padding:20px;margin:25px 0'>
                    <p style='margin:5px 0'><b>Username:</b> {username}</p>
                    <p style='margin:5px 0'><b>Password:</b> {password}</p>
                </div>

                <!-- BUTTON -->
                <div style='text-align:center;margin:30px 0'>
                    <a href='https://localhost:7028/Account/Login'
                       style='background:#2563eb;color:white;padding:12px 28px;
                              text-decoration:none;border-radius:6px;font-weight:bold'>
                        Login to System
                    </a>
                </div>

                <p style='font-size:14px;color:#555'>
                    If you did not request this account, please contact the HR department.
                </p>

            </div>

            <!-- FOOTER -->
            <div style='background:#f8fafc;padding:15px;text-align:center;
                        font-size:12px;color:#777'>
                © {DateTime.Now.Year} Webster Recruitment System
                <br/>
                Automated email — please do not reply
            </div>

        </div>

    </div>"
            };

            mail.To.Add(toEmail);

            smtp.Send(mail);
        }
    }
}