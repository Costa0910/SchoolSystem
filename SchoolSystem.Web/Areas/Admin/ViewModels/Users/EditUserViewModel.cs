using System.ComponentModel.DataAnnotations;

namespace SchoolSystem.Web.Areas.Admin.ViewModels.Users;

public class EditUserViewModel
{
  public string Id { get; set; }

  [MaxLength(50), Display(Name = "First Name"), Required]
  public string FirstName { get; set; }

  [MaxLength(50), Display(Name = "Last Name"), Required]
  public string LastName { get; set; }

  [Display(Name = "Profile Photo")]
  public IFormFile? ProfilePhoto { get; set; }

  public string? ProfilePhotoUrl { get; init; }
}
