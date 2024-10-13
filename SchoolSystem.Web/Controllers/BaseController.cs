using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SchoolSystem.Web.Helpers.Interfaces;
using SchoolSystem.Web.Models;
using SchoolSystem.Web.Models.EnumsClasses;

namespace SchoolSystem.Web.Controllers;

/// <summary>
///  Base controller for all controllers in the application, it contains common logic for all controllers.
///  It is used to get the current user and set the profile photo and current user in the ViewBag.
/// </summary>
/// <param name="userHelper">Helper to get current user with email</param>
public class BaseController(IUserHelper userHelper) : Controller
{
  public override void OnActionExecuting(ActionExecutingContext context)
  {
    base.OnActionExecuting(context);

    if (User.Identity is { IsAuthenticated: true })
    {
      var user = userHelper.GetUserByEmailAsync(User.Identity.Name).Result;
      if (user == null)
      {
        user = new User
        {
          FirstName = "Guest",
          LastName = "User",
          ProfilePhotoId = Guid.Empty
        };
      }

      ViewBag.ProfilePhoto = user.ProfilePhotoId != Guid.Empty
        ? $"https://supershop0910.blob.core.windows.net/{AzureContainerNames.profile}/{user.ProfilePhotoId}"
        : "https://supershop0910.blob.core.windows.net/profile/user.png";

      ViewBag.CurrentUser = user;
    }
  }
}
