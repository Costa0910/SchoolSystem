using SchoolSystem.Web.Helpers.ModelsHelper;

namespace SchoolSystem.Web.Services.Interfaces;

/// <summary>
/// Service for sending emails
/// </summary>
public interface IMailService
{
  Task<ResponseResult> SendEmailAsync(string email, string subject, string body);
  Task<ResponseResult> SendEmailAsync(string email, string subject, string body, string attachmentPath);
}
