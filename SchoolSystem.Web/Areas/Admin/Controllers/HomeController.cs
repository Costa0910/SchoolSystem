using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolSystem.Web.Controllers;
using SchoolSystem.Web.Helpers.Interfaces;
using SchoolSystem.Web.Models.EnumsClasses;

namespace SchoolSystem.Web.Areas.Admin.Controllers;

[Area(Roles.Admin), Authorize(Roles = Roles.Admin)]
public class HomeController(IUserHelper userHelper) : BaseController(userHelper)
{
  public IActionResult Index() => View();
}
