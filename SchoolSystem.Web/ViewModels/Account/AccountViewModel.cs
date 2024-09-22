using SchoolSystem.Web.Models;

namespace SchoolSystem.Web.ViewModels.Account;

public class AccountViewModel
{
  // Common Stuff
  public string ProfilePhotoUrl { get; set; }
  public User User { get; init; }
  public string Role { get; set; } // (e.g., "Admin", "Staff", "Student")

  // Student Stuffs
  public string Status
  {
    get;
    set;
  } // (e.g., "Active", "Graduated","Dropped", "Excluded by Attendance")

  public double AttendancePercentage { get; set; }
  public string PhotoUrl { get; set; }

  // Staff Stuffs
  public DateTime HireDate { get; init; }

  public string Position { get; set; } // (e.g., "Teacher", "Staff", "Principal")

  // Admin Stuffs
  public string AdminType { get; set; } // (e.g., "Super", "Normal")
}
