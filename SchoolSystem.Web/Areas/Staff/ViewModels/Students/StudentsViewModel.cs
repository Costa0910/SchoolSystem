namespace SchoolSystem.Web.Areas.Staff.ViewModels.Students;

public class StudentsViewModel
{
  public Guid Id { get; init; }
  public string ProfilePhotoUrl { get; set; }
  public string Status { get; set; }
  public string UserId { get; set; }
  public string FirstName { get; set; }
  public string LastName { get; set; }
  public string Email { get; set; }
}
