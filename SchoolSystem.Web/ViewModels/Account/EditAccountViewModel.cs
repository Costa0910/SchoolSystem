using System.ComponentModel.DataAnnotations;

namespace SchoolSystem.Web.ViewModels.Account;

public class EditAccountViewModel
{
  [MaxLength(50), Display(Name = "First Name"), Required]
  public string FirstName { get; set; }

  [MaxLength(50), Display(Name = "Last Name"), Required]
  public string LastName { get; set; }

  [Display(Name = "Profile Photo")]
  public IFormFile? ProfilePhoto { get; set; }

  [Display(Name = "Profile Photo")]
  public string? ProfilePhotoUrl { get; set; }
}
