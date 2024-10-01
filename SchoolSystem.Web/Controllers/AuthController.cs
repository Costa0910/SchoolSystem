using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolSystem.Web.Helpers.Interfaces;
using SchoolSystem.Web.Models.EnumsClasses;
using SchoolSystem.Web.Services.Interfaces;
using SchoolSystem.Web.ViewModels.Auth;

namespace SchoolSystem.Web.Controllers;

[AllowAnonymous]
public class AuthController(
  IUserHelper userHelper,
  ICreateMailHtmlHelper
    createMailHtmlHelper,
  IMailService mailService) : Controller
{
  /// <summary>
  ///  Get the user area
  /// </summary>
  /// <returns>return the name of area</returns>
  private string GetUserArea()
  {
    var area = string.Empty;

    if (User.IsInRole(Roles.Admin))
      area = Roles.Admin;
    else if (User.IsInRole(Roles.Staff))
      area = Roles.Staff;
    else if (User.IsInRole(Roles.Student))
      area = Roles.Student;

    return area;
  }

  public async Task<IActionResult> Login(string? ReturnUrl)
  {
    var login = new LoginViewModel() { ReturnUrl = ReturnUrl };

    if (User.Identity is null || !User.Identity.IsAuthenticated)
      return View(login);

    if (User.Identity.Name == null)
      return View(login);

    var user = await userHelper.GetUserByEmailAsync(User.Identity.Name);

    if (user == null)
      return View(login);

    var area = GetUserArea();

    return RedirectToAction("Index", "Home",
      new { area });
  }

  [HttpPost, ValidateAntiForgeryToken]
  public async Task<IActionResult> Login(LoginViewModel model,
    string? ReturnUrl)
  {
    if (!ModelState.IsValid)
      return View(model);

    var user = await userHelper.GetUserByEmailAsync(model.Email);

    if (user is null)
    {
      ModelState.AddModelError(string.Empty, "Email or password is incorrect");

      return View(model);
    }

    var result = await userHelper.CheckPasswordAsync(user, model.Password);

    if (!result)
    {
      ModelState.AddModelError(string.Empty, "Email or password is incorrect");

      return View(model);
    }

    var signInResult
      = await userHelper.LoginAsync(model.Email, model.Password,
        model.RememberMe);

    if (signInResult.Succeeded)
    {
      if (!string.IsNullOrEmpty(ReturnUrl) && Url.IsLocalUrl(ReturnUrl))
        return Redirect(ReturnUrl);

      var area = GetUserArea();

      return RedirectToAction("Index", "Home",
        new { area });
    }

    ModelState.AddModelError(string.Empty, "Email or password is incorrect");

    return View(model);
  }

  public IActionResult RecoverPassword()
  {
    return View();
  }

  [HttpPost, ValidateAntiForgeryToken]
  public async Task<IActionResult> RecoverPassword(
    RecoverPasswordViewModel model)
  {
    if (!ModelState.IsValid)
    {
      ViewBag.Error = "The email is not valid";
      return View(model);
    }

    var user = await userHelper.GetUserByEmailAsync(model.Email);
    if (user is null)
    {
      ViewBag.Error = "The email is not valid";
      return View(model);
    }

    var token = await userHelper.GeneratePasswordResetTokenAsync(user);
    var callbackUrl = Url.Action("ResetPassword", "Auth",
      new { userId = user.Id, token }, Request.Scheme);
    var message =
      $@"<h2>Reset your password</h2><p>To reset your password please click in this link to<a
            href='{callbackUrl}'><strong> Reset your password.</strong></a></p>";

    var mailBody = createMailHtmlHelper.CreateMailBody(user.FirstName, message);
    var response
      = await mailService.SendEmailAsync(user.Email, "Reset your password",
        mailBody);

    if (!response.IsSuccess)
    {
      ViewBag.Error
        = "An error occurred while sending the email, please try again";
      return View(model);
    }

    ViewBag.Message
      = "An email has been sent to your email address with instructions to reset your password";
    return View(model);
  }

  public IActionResult ResetPassword(string userId, string token)
  {
    if (!ModelState.IsValid)
      return RedirectToAction("RecoverPassword");

    var model = new ResetPasswordViewModel
    {
      UserId = userId,
      Token = token
    };

    return View(model);
  }

  [HttpPost, ValidateAntiForgeryToken]
  public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
  {
    if (!ModelState.IsValid)
    {
      ViewBag.Error = "Some data are not valid, please try again";
      return View(model);
    }

    var user = await userHelper.GetUserByIdAsync(model.UserId);
    if (user is null)
    {
      ViewBag.Error = "Some data are not valid, please try again";
      return View(model);
    }

    var result = await userHelper.ResetPasswordAsync(user, model.Token,
      model.Password);

    if (!result.Succeeded)
    {
      ViewBag.Error = "Some data are not valid, please try again";
      return View(model);
    }

    ViewBag.Message
      = "The password has been reset successfully, you can now login with your new password";

    return View();
  }

  public async Task<IActionResult> Logout()
  {
    await userHelper.LogoutAsync();
    return RedirectToAction("Index", "Home");
  }

  public async Task<IActionResult> ConfirmPassword(string userId, string token)
  {
    if (!ModelState.IsValid)
      return RedirectToAction("RecoverPassword");

    var user = await userHelper.GetUserByIdAsync(userId);

    if (user is null)
      return RedirectToAction("Login");

    var result = await userHelper.ConfirmEmailAsync(user, token);

    var newToken = await userHelper.GeneratePasswordResetTokenAsync(user);

    return result.Succeeded
      ? RedirectToAction("ResetPassword", new { userId, token = newToken })
      : RedirectToAction("Login");
  }
}
