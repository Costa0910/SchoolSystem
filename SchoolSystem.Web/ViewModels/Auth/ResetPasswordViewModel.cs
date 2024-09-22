using System.ComponentModel.DataAnnotations;

namespace SchoolSystem.Web.ViewModels.Auth;

public class ResetPasswordViewModel
{
  [Required] public string UserId { get; set; }
  [Required] public string Token { get; set; }

  [Required, DataType(DataType.Password),
   MinLength(8, ErrorMessage = "{0} must be at least {1} characters long"),
   Display(Name = "New Password")]
  public string Password { get; set; }

  [Required, DataType(DataType.Password),
   MinLength(8, ErrorMessage = "{0} must be at least {1} characters long"),
   Compare(nameof
     (Password)), Display(Name = "Confirm New Password")]
  public string ConfirmPassword { get; set; }
}
