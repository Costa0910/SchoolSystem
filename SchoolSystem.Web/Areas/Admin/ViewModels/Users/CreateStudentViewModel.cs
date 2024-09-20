using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SchoolSystem.Web.Areas.Admin.ViewModels.Users;

/// <summary>
/// View model for creating a student.
/// </summary>
public class CreateStudentViewModel : UserViewModel
{
  [Required, DisplayName("Student Photo")]
  public IFormFile Photo { get; set; }
}
