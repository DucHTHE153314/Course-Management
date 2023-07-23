using CourseManagementWebClientWebClient.Models;

namespace CourseManagementWebClientWebClient.Services.EmailService
{
    public interface IEmailService
    {
        Task SendEmailAsync(string toEmail, string subject, string body);
    }

}
