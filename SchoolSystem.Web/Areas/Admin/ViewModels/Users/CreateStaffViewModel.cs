using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SchoolSystem.Web.Areas.Admin.ViewModels.Users;

/// <summary>
/// View model for creating a staff member.
/// </summary>
public class CreateStaffViewModel : UserViewModel
{
  [Required, DisplayName("Position")]
  public string Position { get; set; } // (e.g., "Teacher", "Staff", "Principal")

  public List<string>? PositionOptions { get; set; }
}
