namespace SchoolSystem.Web.Controllers.Api.DTOs;

public class Student
{
  public Guid Id { get; set; }
  public string FullName { get; set; }
  public string PhotoUrl { get; set; }
  public string ProfilePhotoUrl { get; set; }
  public string Email { get; set; }
}
