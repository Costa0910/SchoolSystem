using System.ComponentModel.DataAnnotations;

namespace SchoolSystem.Web.ViewModels.Auth;

public class LoginViewModel
{
    [Required, EmailAddress]
    public string Email { get; set; }

    [Required, MinLength(6, ErrorMessage = "{0} must be at least {1} characters long")]
    public string Password { get; set; }

    public bool RememberMe { get; set; }

    public string? ReturnUrl { get; set; }
}
