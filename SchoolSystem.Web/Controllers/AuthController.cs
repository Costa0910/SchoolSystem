using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolSystem.Web.Helpers.Interfaces;
using SchoolSystem.Web.Models.EnumsClasses;
using SchoolSystem.Web.ViewModels.Auth;

namespace SchoolSystem.Web.Controllers;

[AllowAnonymous]
public class AuthController(IUserHelper userHelper) : Controller
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

  public async Task<IActionResult> Login()
  {
    if (User.Identity is null || !User.Identity.IsAuthenticated)
      return View();

    if (User.Identity.Name == null)
      return View();

    var user = await userHelper.GetUserByEmailAsync(User.Identity.Name);

    if (user == null)
      return View();

    var area = GetUserArea();

    return RedirectToAction("Index", "Home",
      new { area = area });
  }

  [HttpPost, ValidateAntiForgeryToken]
  public async Task<IActionResult> Login(LoginViewModel model, string? ReturnUrl)
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
        new { area = area });
    }

    ModelState.AddModelError(string.Empty, "Email or password is incorrect");

    return View(model);
  }

  public IActionResult RecoverPassword()
  {
    throw new NotImplementedException();
  }
}
