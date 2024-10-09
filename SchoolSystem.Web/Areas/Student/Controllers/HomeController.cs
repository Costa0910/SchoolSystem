using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolSystem.Web.Controllers;
using SchoolSystem.Web.Data.Interfaces;
using SchoolSystem.Web.Helpers.Interfaces;
using SchoolSystem.Web.Models.EnumsClasses;
using SchoolSystem.Web.ViewModels.Home;

namespace SchoolSystem.Web.Areas.Student.Controllers;

[Area(Roles.Student), Authorize(Roles = Roles.Student)]
public class HomeController(IUserHelper userHelper, IMapper mapper, IAlertRepository alertRepository) :
  BaseController
  (userHelper)
{
  public async Task<IActionResult> Index()
  {
    var alerts = await alertRepository.GetAlertsAsync(Roles.Student);

    var model = new HomeViewModel
    {
      InBoxAlerts = mapper.Map<IEnumerable<AlertViewModel>>(alerts),
    };

    return View(model);
  }
}
