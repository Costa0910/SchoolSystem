using System.Diagnostics;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolSystem.Web.Areas.Admin.ViewModels.Users;
using SchoolSystem.Web.Controllers;
using SchoolSystem.Web.Data.Interfaces;
using SchoolSystem.Web.Helpers.Interfaces;
using SchoolSystem.Web.Models;
using SchoolSystem.Web.Models.EnumsClasses;
using SchoolSystem.Web.Services.Interfaces;

namespace SchoolSystem.Web.Areas.Admin.Controllers
{
  [Area(Roles.Admin), Authorize(Roles = Roles.Admin)]
  public class UsersController : BaseController
  {
    private readonly IUserHelper _userHelper;
    private readonly IMapper _mapper;
    private readonly IMailService _mailService;
    private readonly ICreateMailHtmlHelper _createMailHtmlHelper;
    private readonly IBlobStorageService _blobStorageService;
    private readonly IAdminRepository _adminRepository;
    private readonly IStudentRepository _studentRepository;
    private readonly IStaffRepository _staffRepository;

    public UsersController(IUserHelper userHelper, IMapper mapper,
      IMailService mailService, ICreateMailHtmlHelper createMailHtmlHelper,
      IBlobStorageService blobStorageService, IAdminRepository adminRepository,
      IStudentRepository studentRepository,
      IStaffRepository staffRepository) : base(userHelper)
    {
      _userHelper = userHelper;
      _mapper = mapper;
      _mailService = mailService;
      _createMailHtmlHelper = createMailHtmlHelper;
      _blobStorageService = blobStorageService;
      _adminRepository = adminRepository;
      _studentRepository = studentRepository;
      _staffRepository = staffRepository;
    }

    public async Task<IActionResult> Index(string? message)
    {
      if (!string.IsNullOrEmpty(message))
      {
        ViewBag.Message = message;
      }

      var users = await _userHelper.GetAllUsersAsync();

      users = users.Where(u => u.Email != User?.Identity?.Name);

      var usersList = new List<ViewUserViewModel>();

      foreach (var user in users)
      {
        var role = await _userHelper.GetRolesAsync(user);
        var userViewModel = _mapper.Map<ViewUserViewModel>(user);
        userViewModel.Role = role.FirstOrDefault();
        usersList.Add(userViewModel);
      }

      return View(usersList);
    }


    private async Task<User?> CreateUser(UserViewModel model)
    {
      const string password = "Password@123#"; // User gonna change it later
      var profileImageId = Guid.Empty;

      if (model.Role is null) return null;

      if (model.ProfilePhoto is not null && model.ProfilePhoto.Length > 0)
      {
        profileImageId = await _blobStorageService.UploadFileAsync(model
          .ProfilePhoto, AzureContainerNames.profile);
      }

      var user = _mapper.Map<User>(model);
      user.ProfilePhotoId = profileImageId;
      user.UserName = model.Email;

      var newUser = await _userHelper.AddUserAsync(user, password, model.Role);

      return newUser;
    }

    private async Task<bool> SendMailToUser(User user)
    {
      var token = await _userHelper.GenerateEmailConfirmationTokenAsync(user);
      var callbackUrl = Url.Action("ConfirmPassword", "Auth", new
      {
        userId = user.Id, token, area = string.Empty
      }, Request.Scheme);
      var message =
        $@"<h2>Welcome to EduSmart</h2><p>Your account has been created successfully. Please click in this link to<a
                href='{callbackUrl}'><strong> active your account and change your password.</strong></a></p>";

      var mailBody
        = _createMailHtmlHelper.CreateMailBody(user.FirstName, message);
      var response = await _mailService.SendEmailAsync(user.Email,
        "Active your account", mailBody);

      return response.IsSuccess;
    }

    public IActionResult CreateAdmin()
    {
      return View();
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateAdmin(CreateAdminViewModel model)
    {
      if (!ModelState.IsValid) return View(model);


      var userModel = _mapper.Map<UserViewModel>(model);
      userModel.Role = Roles.Admin;
      var user = await CreateUser(userModel);
      if (user is null)
      {
        ViewBag.Error = "User not created";
        return View(model);
      }

      var admin = new AdminUser
      {
        Id = Guid.NewGuid(),
        User = user,
        AdminType = AdminType.Normal,
        Status = AdminAndStaffStatus.Active
      };

      await _adminRepository.AddAsync(admin);
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

    public IActionResult CreateStaff()
    {
      var model = new CreateStaffViewModel
      {
        PositionOptions = StaffType.GetStaffTypes()
      };

      return View(model);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateStaff(CreateStaffViewModel model)
    {
      if (!ModelState.IsValid)
      {
        ViewBag.Error = "Please fill all the fields";
        model.PositionOptions = StaffType.GetStaffTypes();
        return View(model);
      }

      var userModel = _mapper.Map<UserViewModel>(model);
      userModel.Role = Roles.Staff;
      var user = await CreateUser(userModel);
      if (user is null)
      {
        ViewBag.Error = "User not created";
        model.PositionOptions = StaffType.GetStaffTypes();
        return View(model);
      }

      var staff = new Models.Staff
      {
        Id = Guid.NewGuid(),
        User = user,
        Position = model.Position,
        Status = AdminAndStaffStatus.Active,
        HireDate = DateTime.Now
      };

      await _staffRepository.AddAsync(staff);
      var sendMailResult = await SendMailToUser(user);

      if (!sendMailResult)
      {
        ViewBag.Error
          = "User created successfully, but the email was not sent.";
        model.PositionOptions = StaffType.GetStaffTypes();
        return View(model);
      }

      return RedirectToAction("Index",
        new { message = $"{user.FirstName} created successfully" });
    }

    public IActionResult CreateStudent()
    {
      return View();
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateStudent(CreateStudentViewModel model)
    {
      if (!ModelState.IsValid) return View(model);

      var userModel = _mapper.Map<UserViewModel>(model);
      userModel.Role = Roles.Student;
      var user = await CreateUser(userModel);
      if (user is null)
      {
        ViewBag.Error = "User not created";
        return View(model);
      }

      var photoId = Guid.Empty;
      if (model.Photo.Length > 0)
      {
        photoId = await _blobStorageService.UploadFileAsync(model
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

      await _studentRepository.AddAsync(student);
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
        return RedirectToAction("Index", new { message = "User not found" });
      }

      var user = await _userHelper.GetUserByIdAsync(id);
      if (user is null)
      {
        return RedirectToAction("Index", new { message = "User not found" });
      }

      var userViewModel = _mapper.Map<EditUserViewModel>(user);

      return View(userViewModel);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(EditUserViewModel model)
    {
      if (!ModelState.IsValid) return View(model);

      var user = await _userHelper.GetUserByIdAsync(model.Id);
      if (user is null)
      {
        return RedirectToAction("Index", new { message = "User not found" });
      }

      var profileImageId = user.ProfilePhotoId;

      if (model.ProfilePhoto is not null && model.ProfilePhoto.Length > 0)
      {
        if (profileImageId is not null && profileImageId != Guid.Empty)
        {
          await _blobStorageService.DeleteFileAsync(profileImageId.Value,
            AzureContainerNames.profile);
        }

        profileImageId = await _blobStorageService.UploadFileAsync(model
          .ProfilePhoto, AzureContainerNames.profile);
      }

      user = _mapper.Map(model, user);
      user.ProfilePhotoId = profileImageId;

      var result = await _userHelper.UpdateUserAsync(user);

      if (!result.Succeeded)
      {
        ViewBag.Error = "User couldn't be updated, please try again";
        return View(model);
      }

      return RedirectToAction("Index",
        new { message = $"{user.FirstName} updated successfully" });
    }

    public async Task<IActionResult> Details(string id, string role)
    {
      if (!ModelState.IsValid)
      {
        return RedirectToAction("Index", new { message = "User not found" });
      }

      UserDetailsViewModel user;

      if (role == Roles.Admin)
      {
        var admin = await _adminRepository
          .GetAdminUserByIdIncludeUserAsync(id);
        user = _mapper.Map<UserDetailsViewModel>(admin);
      }
      else if (role == Roles.Staff)
      {
        var staff = await _staffRepository
          .GetStaffByIdIncludeUserAsync(id);
        user = _mapper.Map<UserDetailsViewModel>(staff);
      }
      else if (role == Roles.Student)
      {
        var student = await _studentRepository
          .GetStudentByIdIncludeUserAsync(id);
        user = _mapper.Map<UserDetailsViewModel>(student);
      }
      else
      {
        return RedirectToAction("Index", new { message = "User not found" });
      }

      user.Role = role;
      return View(user);
    }

    public async Task<IActionResult> Delete(string id, string role)
    {
      if (!ModelState.IsValid)
      {
        return RedirectToAction("Index", new { message = "User not found" });
      }

      var user = await _userHelper.GetUserByIdAsync(id);
      if (user is null)
      {
        return RedirectToAction("Index", new { message = "User not found" });
      }

      // check if possible to delete without error
      try
      {
        var result = await _userHelper.DeleteUserAsync(user);
        if (!result.Succeeded)
        {
          return RedirectToAction("Index",
            new { message = "User not deleted, try again" });
        }
      }
      catch (Exception ex)
      {
        if (ex.InnerException is not null &&
            ex.InnerException.Message.Contains("dbo.Admins") ||
            ex.InnerException.Message.Contains("dbo.Staffs") ||
            ex.InnerException.Message.Contains("dbo.Students"))
        {
          var r = await DeleteFromSpecificTable(role, user);
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

        throw new DbUpdateException("An error occurred while deleting the user",
          ex);
      }

      return RedirectToAction("Index",
        new { message = $"{user.FirstName} deleted successfully" });
    }

    private async Task<bool> DeleteFromSpecificTable(string role, User user)
    {
      bool r;

      if (role == Roles.Admin)
      {
        r = await _adminRepository.DeleteAdminUserAsync(user.Id);
        if (!r) return false;
      }
      else if (role == Roles.Staff)
      {
        r = await _staffRepository.DeleteStaffAsync(user.Id);
        if (!r) return false;
      }
      else if (role == Roles.Student)
      {
        var student
          = await _studentRepository.GetStudentByIdIncludeUserAsync(user.Id);
        if (student is null) return false;
        var canDelete = await _studentRepository.CanDeleteStudentAsync(student);
        if (!canDelete)
        {
          throw new DbUpdateException(
            "DELETE statement conflicted with the REFERENCE constraint",
            new Exception("DELETE statement conflicted"));
        }

        r = await _studentRepository.DeleteStudentAsync(user.Id);
        if (!r) return false;
      }

      if (user.ProfilePhotoId is not null && user.ProfilePhotoId != Guid.Empty)
      {
        await _blobStorageService.DeleteFileAsync(user.ProfilePhotoId.Value,
          AzureContainerNames.profile);
      }

      await _userHelper.DeleteUserAsync(user);
      return true;
    }
  }
}
