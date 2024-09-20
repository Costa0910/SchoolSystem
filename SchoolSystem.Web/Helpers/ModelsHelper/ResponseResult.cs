namespace SchoolSystem.Web.Helpers.ModelsHelper;

/// <summary>
/// Represents the result of an operation
/// </summary>
public class ResponseResult
{
  public bool IsSuccess { get; set; }
  public string? Message { get; set; }
}
