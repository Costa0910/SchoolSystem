using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolSystem.Web.Models.EnumsClasses;

namespace SchoolSystem.Web.Areas.Admin.Controllers;

[Area(Roles.Admin), Authorize(Roles = Roles.Admin)]
public class HomeController : Controller
{
  public IActionResult Index() => View();
}
