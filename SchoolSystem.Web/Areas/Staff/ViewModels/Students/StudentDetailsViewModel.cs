using SchoolSystem.Web.Models;

namespace SchoolSystem.Web.Areas.Staff.ViewModels.Students;

public class StudentDetailsViewModel
{
  public string ProfilePhotoUrl { get; set; }
  public User User { get; init; }
  public string Role { get; set; }

  public string Status { get; set; }

  public double AttendancePercentage { get; set; }
  public string PhotoUrl { get; set; }
}
