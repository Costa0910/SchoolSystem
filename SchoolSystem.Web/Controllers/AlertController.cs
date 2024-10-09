using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolSystem.Web.Data.Interfaces;
using SchoolSystem.Web.Helpers.Interfaces;
using SchoolSystem.Web.Models;
using SchoolSystem.Web.Models.EnumsClasses;
using SchoolSystem.Web.ViewModels.Alerts;


namespace SchoolSystem.Web.Controllers;

/// <summary>
/// Controller for managing alerts.
/// </summary>
[Authorize(Roles = $"{Roles.Staff},{Roles.Admin}")]
public class AlertController(
  IAlertRepository alertRepository,
  IUserHelper userHelper) :
  Controller
{
  [HttpPost]
  public async Task<IActionResult> Create([FromBody] CreateAlert alert)
  {
    if (!ModelState.IsValid)
    {
      return BadRequest();
    }

    var user = await userHelper.GetUserByEmailAsync(User.Identity.Name);
    if (user == null)
    {
      return BadRequest();
    }

    var sendTo = alert.SendTo switch
    {
      "Students" => Roles.Student,
      "Admins" => Roles.Admin,
      "Staffs" => Roles.Staff,
      _ => $"{Roles.Admin},{Roles.Staff},{Roles.Student}" // All roles
    };

    var newAlert = new Alert
    {
      Id = Guid.NewGuid(),
      Message = alert.Message,
      SendTo = sendTo,
      CreatedBy = user,
      DateCreated = DateTime.Now
    };

    await alertRepository.AddAsync(newAlert);

    return Ok(new
    {
      newAlert.Id,
      newAlert.Message,
      DateCreated = newAlert.DateCreated.ToString("MMM d, yyyy HH:mm"),
    });
  }
}
