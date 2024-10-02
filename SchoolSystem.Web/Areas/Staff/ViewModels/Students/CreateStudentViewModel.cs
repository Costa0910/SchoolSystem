using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SchoolSystem.Web.Areas.Staff.ViewModels.Students;

/// <summary>
/// View model for creating a student.
/// </summary>
public class CreateStudentViewModel
{
  [MaxLength(50), Display(Name = "First Name"), Required]
  public string FirstName { get; set; }

  [MaxLength(50), Display(Name = "Last Name"), Required]
  public string LastName { get; set; }

  [Required, Display(Name = "Email"), EmailAddress]
  public string Email { get; set; }

  [Display(Name = "Profile Photo")] public IFormFile? ProfilePhoto { get; set; }

  [Required(ErrorMessage = "Student must have photo"),
   DisplayName("Student Photo")]
  public IFormFile Photo { get; set; }
}
