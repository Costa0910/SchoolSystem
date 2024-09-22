using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolSystem.Web.Data.Interfaces;
using SchoolSystem.Web.Helpers.Interfaces;
using SchoolSystem.Web.Models.EnumsClasses;
using SchoolSystem.Web.Services.Interfaces;
using SchoolSystem.Web.ViewModels.Account;

namespace SchoolSystem.Web.Controllers;

[Authorize]
public class AccountController(
  IUserHelper userHelper,
  IStudentRepository
    studentRepository,
  IStaffRepository staffRepository,
  IAdminRepository
    adminRepository,
  IMapper mapper,
  IBlobStorageService blobStorageService)
  : Controller
{
  public async Task<IActionResult> Index()
  {
    AccountViewModel user;

    if (User.IsInRole(Roles.Admin))
    {
      var adminUser = await adminRepository.GetAdminUserByUserEmailAsync(User
        .Identity
        .Name);
      user = mapper.Map<AccountViewModel>(adminUser);
    }
    else if (User.IsInRole(Roles.Staff))
    {
      var staffUser = await staffRepository.GetStaffByUserEmailAsync(User
        .Identity
        .Name);
      user = mapper.Map<AccountViewModel>(staffUser);
    }
    else if (User.IsInRole(Roles.Student))
    {
      var studentUser = await studentRepository.GetStudentByUserEmailAsync(User
        .Identity
        .Name);
      user = mapper.Map<AccountViewModel>(studentUser);
    }
    else
    {
      return RedirectToAction("Login", "Auth");
    }

    if (user is null)
    {
      return RedirectToAction("Login", "Auth");
    }

    return View(user);
  }

  public async Task<IActionResult> SettingsEdit()
  {
    var user = await userHelper.GetUserByEmailAsync(User.Identity.Name);

    if (user is null)
    {
      return RedirectToAction("Login", "Auth");
    }

    var model = mapper.Map<EditAccountViewModel>(user);

    return View(model);
  }

  [HttpPost, ValidateAntiForgeryToken]
  public async Task<IActionResult> SettingsEdit(EditAccountViewModel model)
  {
    if (!ModelState.IsValid)
    {
      ViewBag.Error = "Invalid data, please try again.";
      return View(model);
    }

    var user = await userHelper.GetUserByEmailAsync(User.Identity.Name);

    if (user is null)
    {
      return RedirectToAction("Login", "Auth");
    }

    var profileImageId = user.ProfilePhotoId;


    if (model.ProfilePhoto is not null && model.ProfilePhoto.Length > 0)
    {
      if (profileImageId is not null && profileImageId != Guid.Empty)
      {
        await blobStorageService.DeleteFileAsync(profileImageId.Value,
          AzureContainerNames.profile);
      }

      profileImageId = await blobStorageService.UploadFileAsync(
        model.ProfilePhoto,
        AzureContainerNames.profile);
    }

    user.FirstName = model.FirstName;
    user.LastName = model.LastName;
    user.ProfilePhotoId = profileImageId;

    var result = await userHelper.UpdateUserAsync(user);

    if (!result.Succeeded)
    {
      ViewBag.Error = "Error updating data, please try again.";
      return View(model);
    }

    var newModel = mapper.Map<EditAccountViewModel>(user);

    ViewBag.Message = "Data updated successfully.";
    return View(newModel);
  }

  public IActionResult ChangePassword()
  {
    return View();
  }

  [HttpPost, ValidateAntiForgeryToken]
  public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
  {
    if (!ModelState.IsValid)
    {
      ViewBag.Error = "Invalid data, please try again.";
      return View();
    }

    var user = await userHelper.GetUserByEmailAsync(User.Identity.Name);

    if (user is null)
    {
      return RedirectToAction("Login", "Auth");
    }

    var result = await userHelper.ChangePasswordAsync(user,
      model.CurrentPassword,
      model
        .NewPassword);

    if (!result.Succeeded)
    {
      ViewBag.Error = "Error changing password, please try again.";
      return View();
    }

    ViewBag.Message = "Password changed successfully.";
    return View();
  }

  /// <summary>
  ///  Auth to be done in auth controller, try to configure in Program.cs but didn't work
  /// </summary>
  /// <param name="ReturnUrl">Path to return user after login</param>
  /// <returns>redirect to AuthController</returns>
  [AllowAnonymous]
  public IActionResult Login(string? ReturnUrl)
  {
    return RedirectToAction("Login", "Auth", new { ReturnUrl });
  }
}
