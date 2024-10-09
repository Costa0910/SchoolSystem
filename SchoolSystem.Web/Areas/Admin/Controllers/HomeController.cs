using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolSystem.Web.Controllers;
using SchoolSystem.Web.Data.Interfaces;
using SchoolSystem.Web.Helpers.Interfaces;
using SchoolSystem.Web.Models.EnumsClasses;
using SchoolSystem.Web.ViewModels.Home;

namespace SchoolSystem.Web.Areas.Admin.Controllers;

[Area(Roles.Admin), Authorize(Roles = Roles.Admin)]
public class HomeController(
  IUserHelper userHelper,
  IAlertRepository alertRepository,
  IMapper mapper) :
  BaseController
  (userHelper)
{
  private readonly IUserHelper _userHelper = userHelper;

  public async Task<IActionResult> Index()
  {
    var user = await _userHelper.GetUserByEmailAsync(User.Identity.Name);

    if (user == null)
    {
      return RedirectToAction("Login", "Auth");
    }

    var alerts = await alertRepository.GetAlertsAsync(Roles.Admin);
    var alertsCreated = await alertRepository.GetAlertsCreatedByUser(user);

    var alertsNotByUser = alerts.Where(a => a.CreatedBy != user);

    var model = new HomeViewModel
    {
      InBoxAlerts = mapper.Map<IEnumerable<AlertViewModel>>(alertsNotByUser),
      AlertsByUser = mapper.Map<IEnumerable<AlertViewModel>>(alertsCreated),
    };

    return View(model);
  }
}
