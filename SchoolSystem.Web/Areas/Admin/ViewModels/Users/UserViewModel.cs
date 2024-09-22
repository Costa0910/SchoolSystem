using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace SchoolSystem.Web.Areas.Admin.ViewModels.Users;

/// <summary>
///  View model for creating a user.
/// </summary>
public class UserViewModel
{
  [MaxLength(50), Display(Name = "First Name"), Required]
  public string FirstName { get; set; }

  [MaxLength(50), Display(Name = "Last Name"), Required]
  public string LastName { get; set; }

  [Required, Display(Name = "Email"), EmailAddress]
  public string Email { get; set; }

  [Display(Name = "Profile Photo")]
  public IFormFile? ProfilePhoto { get; set; }

  [Display(Name = "Role")]
  public string? Role { get; set; }
}
