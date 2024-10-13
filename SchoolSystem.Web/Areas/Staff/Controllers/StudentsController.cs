using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolSystem.Web.Areas.Staff.ViewModels.Students;
using SchoolSystem.Web.Controllers;
using SchoolSystem.Web.Data.Interfaces;
using SchoolSystem.Web.Helpers.Interfaces;
using SchoolSystem.Web.Models;
using SchoolSystem.Web.Models.EnumsClasses;
using SchoolSystem.Web.Services.Interfaces;

namespace SchoolSystem.Web.Areas.Staff.Controllers;

[Area(Roles.Staff), Authorize(Roles = Roles.Staff)]
public class StudentsController(
  IUserHelper userHelper,
  IStudentRepository
    studentRepository,
  IBlobStorageService blobStorageService,
  IMailService mailService,
  ICreateMailHtmlHelper createMailHtmlHelper,
  IMapper mapper) : BaseController
  (userHelper)
{
  public async Task<IActionResult> Index(string? message)
  {
    if (!string.IsNullOrEmpty(message))
    {
      ViewBag.Message = message;
    }

    var students = await studentRepository.GetStudentsIncludeUserAsync();

    var studentsViewModel = students.Select(mapper.Map<StudentsViewModel>);

    return View(studentsViewModel);
  }

  /// <summary>
  ///  Send an email to the user to confirm the account.
  /// </summary>
  /// <param name="user">User to sent email</param>
  /// <returns>true if email was send else false</returns>
  private async Task<bool> SendMailToUser(User user)
  {
    var token = await userHelper.GenerateEmailConfirmationTokenAsync(user);
    var callbackUrl = Url.Action("ConfirmPassword", "Auth", new
    {
      userId = user.Id, token, area = string.Empty
    }, Request.Scheme);
    var message =
      $@"<h2>Welcome to EduSmart</h2><p>Your account has been created successfully. Please click in this link to<a
                href='{callbackUrl}'><strong> active your account and change your password.</strong></a></p>";

    var mailBody
      = createMailHtmlHelper.CreateMailBody(user.FirstName, message);
    var response = await mailService.SendEmailAsync(user.Email,
      "Active your account", mailBody);

    return response.IsSuccess;
  }

  public IActionResult Create() => View();

  [HttpPost, ValidateAntiForgeryToken]
  public async Task<IActionResult> Create(CreateStudentViewModel model)
  {
    if (!ModelState.IsValid)
    {
      return View(model);
    }

    const string password = "Password@123#"; // User gonna change it later
    var profileImageId = Guid.Empty;


    if (model.ProfilePhoto is not null && model.ProfilePhoto.Length > 0)
    {
      profileImageId = await blobStorageService.UploadFileAsync(model
        .ProfilePhoto, AzureContainerNames.profile);
    }

    var user = mapper.Map<User>(model);
    user.ProfilePhotoId = profileImageId;
    user.UserName = model.Email;

    var newUser = await userHelper.AddUserAsync(user, password, Roles.Student);

    if (newUser is null)
    {
      ViewBag.ErrorMessage = "Something went wrong, please try again.";
      return View(model);
    }

    var photoId = Guid.Empty;
    if (model.Photo.Length > 0)
    {
      photoId = await blobStorageService.UploadFileAsync(model
        .Photo, AzureContainerNames.profile);
    }

    var student = new Models.Student
    {
      Id = Guid.NewGuid(),
      User = user,
      Status = StatusStudent.Active,
      AttendancePercentage = 100,
      Courses = [],
      PhotoId = photoId
    };

    await studentRepository.AddAsync(student);
    var sendMailResult = await SendMailToUser(user);

    if (!sendMailResult)
    {
      ViewBag.Error
        = "User created successfully, but the email was not sent.";
      return View(model);
    }

    return RedirectToAction("Index",
      new { message = $"{user.FirstName} created successfully" });
  }

  public async Task<IActionResult> Edit(string id)
  {
    if (!ModelState.IsValid)
    {
      return RedirectToAction("Index", new { message = "Student not found" });
    }

    var user = await userHelper.GetUserByIdAsync(id);
    if (user is null)
    {
      return RedirectToAction("Index", new { message = "Student not found" });
    }

    var userViewModel = mapper.Map<EditStudentViewModel>(user);

    return View(userViewModel);
  }

  [HttpPost, ValidateAntiForgeryToken]
  public async Task<IActionResult> Edit(EditStudentViewModel model)
  {
    if (!ModelState.IsValid) return View(model);

    var user = await userHelper.GetUserByIdAsync(model.Id);
    if (user is null)
    {
      return RedirectToAction("Index", new { message = "student not found" });
    }

    var profileImageId = user.ProfilePhotoId;

    if (model.ProfilePhoto is not null && model.ProfilePhoto.Length > 0)
    {
      if (profileImageId is not null && profileImageId != Guid.Empty)
      {
        await blobStorageService.DeleteFileAsync(profileImageId.Value,
          AzureContainerNames.profile);
      }

      profileImageId = await blobStorageService.UploadFileAsync(model
        .ProfilePhoto, AzureContainerNames.profile);
    }

    user = mapper.Map(model, user);
    user.ProfilePhotoId = profileImageId;

    var result = await userHelper.UpdateUserAsync(user);

    if (!result.Succeeded)
    {
      ViewBag.Error = "User couldn't be updated, please try again";
      return View(model);
    }

    return RedirectToAction("Index",
      new { message = $"{user.FirstName} updated successfully" });
  }

  public async Task<IActionResult> Details(string id)
  {
    if (!ModelState.IsValid)
    {
      return RedirectToAction("Index", new { message = "User not found" });
    }

    var student = await studentRepository
      .GetStudentByIdIncludeUserAsync(id);
    var user = mapper.Map<StudentDetailsViewModel>(student);
    return View(user);
  }

  public async Task<IActionResult> Delete(string id)
  {
    if (!ModelState.IsValid)
    {
      return RedirectToAction("Index", new { message = "User not found" });
    }

    var user = await userHelper.GetUserByIdAsync(id);
    if (user is null)
    {
      return RedirectToAction("Index", new { message = "User not found" });
    }

    var student = await studentRepository.GetStudentByIdIncludeUserAsync(id);

    if (student is null)
    {
      return RedirectToAction("Index", new { message = "Student not found" });
    }

    var canDelete = await studentRepository.CanDeleteStudentAsync(student);

    if (!canDelete)
    {
      throw new DbUpdateException(
        "DELETE statement conflicted with the REFERENCE constraint",
        new Exception(
          "DELETE statement conflicted with the REFERENCE constraint"));
    }

    // check if possible to delete without error
    try
    {
      var result = await userHelper.DeleteUserAsync(user);
      if (!result.Succeeded)
      {
        return RedirectToAction("Index",
          new { message = "User not deleted, try again" });
      }
    }
    catch (Exception ex)
    {
      if (ex.InnerException is not null &&
          ex.InnerException.Message.Contains("dbo.Students"))
      {
        var r = await DeleteFromStudentTable(user);
        if (r)
        {
          return RedirectToAction("Index",
            new { message = $"{user.FirstName} deleted successfully" });
        }
        else
        {
          return RedirectToAction("Index",
            new { message = "User can't be deleted" });
        }
      }

      throw new DbUpdateException("Error deleting user", ex);
    }

    return RedirectToAction("Index",
      new { message = $"{user.FirstName} deleted successfully" });
  }

  private async Task<bool> DeleteFromStudentTable(User user)
  {
    var result = await studentRepository.DeleteStudentAsync(user.Id);
    if (!result)
      return false;

    if (user.ProfilePhotoId is not null && user.ProfilePhotoId != Guid.Empty)
    {
      await blobStorageService.DeleteFileAsync(user.ProfilePhotoId.Value,
        AzureContainerNames.profile);
    }

    await userHelper.DeleteUserAsync(user);
    return true;
  }
}
