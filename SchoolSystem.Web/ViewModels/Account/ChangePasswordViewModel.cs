using System.ComponentModel.DataAnnotations;

namespace SchoolSystem.Web.ViewModels.Account;

public class ChangePasswordViewModel
{
  [Required,
   MinLength(8, ErrorMessage = "The {0} must be at least {1} characters long."),
   Display(Name = "Current Password")]
  public string CurrentPassword { get; set; }

  [Required,
   MinLength(8, ErrorMessage = "The {0} must be at least {1} characters long."),
   Display(Name = "New Password")]
  public string NewPassword { get; set; }

  [Compare("NewPassword"), Required,
   MinLength(8, ErrorMessage = "The {0} must be at least {1} characters long."),
   Display(Name = "Confirm New Password")]
  public string ConfirmPassword { get; set; }
}
