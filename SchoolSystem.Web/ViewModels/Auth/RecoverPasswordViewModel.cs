using System.ComponentModel.DataAnnotations;

namespace SchoolSystem.Web.ViewModels.Auth;

public class RecoverPasswordViewModel
{
  [Required, EmailAddress] public string Email { get; set; }
}
