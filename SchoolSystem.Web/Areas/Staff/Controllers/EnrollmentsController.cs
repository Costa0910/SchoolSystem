using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolSystem.Web.Controllers;
using SchoolSystem.Web.Helpers.Interfaces;
using SchoolSystem.Web.Models.EnumsClasses;

namespace SchoolSystem.Web.Areas.Staff.Controllers;

[Area(Roles.Staff), Authorize(Roles = Roles.Staff)]
public class EnrollmentsController(IUserHelper userHelper)
  : BaseController(userHelper)
{
}
