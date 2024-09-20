using MailKit.Net.Smtp;
using MimeKit;
using SchoolSystem.Web.Helpers.ModelsHelper;
using SchoolSystem.Web.Services.Interfaces;

namespace SchoolSystem.Web.Services;

/// <summary>
/// Service for sending emails
/// </summary>
/// <param name="configuration">To get variables defines in app settings.json</param>
public class MailService(IConfiguration configuration) : IMailService
{
    public async Task<ResponseResult> SendEmailAsync(string email, string subject, string body)
    {
        var displayName = configuration["EmailSettings:DisplayName"];
        var emailFrom = configuration["EmailSettings:Email"];
        var password = configuration["EmailSettings:Password"];
        var smtp = configuration["EmailSettings:Smtp"];
        var port = int.Parse(
            configuration["EmailSettings:Port"] ??
            throw new ArgumentNullException(nameof(configuration), "EmailSettings:Port is not set in appsettings.json"));

        var message = new MimeMessage();

        message.From.Add(new MailboxAddress(displayName, emailFrom));
        message.To.Add(new MailboxAddress(email, email));
        message.Subject = subject;

        var bodyBuilder = new BodyBuilder()
        {
            HtmlBody = body
        };

        message.Body = bodyBuilder.ToMessageBody();

        try
        {
            using var client = new SmtpClient();
            await client.ConnectAsync(smtp, port, false);
            await client.AuthenticateAsync(emailFrom, password);
            await client.SendAsync(message);
            await client.DisconnectAsync(true);

            return new ResponseResult { IsSuccess = true };
        }
        catch (Exception e)
        {
            return new ResponseResult { IsSuccess = false, Message = e.Message };
        }
    }

    public async Task<ResponseResult> SendEmailAsync(string email, string subject, string body, string attachmentPath)
    {
        var displayName = configuration["EmailSettings:DisplayName"];
        var emailFrom = configuration["EmailSettings:Email"];
        var password = configuration["EmailSettings:Password"];
        var smtp = configuration["EmailSettings:Smtp"];
        var port = int.Parse(
            configuration["EmailSettings:Port"] ??
            throw new ArgumentNullException(nameof(configuration), "EmailSettings:Port is not set in appsettings.json"));

        var message = new MimeMessage();

        message.From.Add(new MailboxAddress(displayName, emailFrom));
        message.To.Add(new MailboxAddress(email, email));
        message.Subject = subject;

        var bodyBuilder = new BodyBuilder()
        {
            HtmlBody = body
        };

        message.Body = bodyBuilder.ToMessageBody();

        var attachment = new MimePart("application", "octet-stream")
        {
            Content = new MimeContent(File.OpenRead(attachmentPath), ContentEncoding.Default),
            ContentDisposition = new ContentDisposition(ContentDisposition.Attachment),
            ContentTransferEncoding = ContentEncoding.Base64,
            FileName = Path.GetFileName(attachmentPath)
        };

        message.Body = new Multipart("mixed")
        {
            bodyBuilder.ToMessageBody(),
            attachment
        };

        try
        {
            using var client = new SmtpClient();
            await client.ConnectAsync(smtp, port, false);
            await client.AuthenticateAsync(email, password);
            await client.SendAsync(message);
            await client.DisconnectAsync(true);

            return new ResponseResult { IsSuccess = true };
        }
        catch (Exception e)
        {
            return new ResponseResult { IsSuccess = false, Message = e.Message };
        }
    }
}
