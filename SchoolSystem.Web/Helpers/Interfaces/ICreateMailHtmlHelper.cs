namespace SchoolSystem.Web.Helpers.Interfaces;

/// <summary>
/// Helper for creating HTML for emails
/// </summary>
public interface ICreateMailHtmlHelper
{
  string CreateMailBody(string name, string message);
}
