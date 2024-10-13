using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolSystem.Web.Helpers.Interfaces;

namespace SchoolSystem.Web.Controllers;

[AllowAnonymous]
[Route("Error")]
public class ErrorController(
  ILogger<ErrorController> logger,
  IUserHelper userHelper)
  : BaseController(userHelper)
{
  private bool IsAuthenticated => User.Identity is { IsAuthenticated: true };

  private bool IsForeignKeyConstraintViolation(DbUpdateException ex)
  {
    // Check if the exception is related to a foreign key constraint violation
    // or a related entity that is being used
    return ex.InnerException != null &&
           (ex.InnerException.Message.Contains("FOREIGN KEY constraint") ||
            ex.InnerException.Message.Contains("DELETE statement conflicted")
            || ex.InnerException.Message.Contains("deleting the user"));
  }

  [Route("Exception")]
  public IActionResult Exception()
  {
    var exceptionHandlerPathFeature
      = HttpContext.Features.Get<IExceptionHandlerPathFeature>();

    logger.LogError(
      exceptionHandlerPathFeature?.Error,
      "An error occurred while processing your request");

    // Retrieve the exception details
    var exceptionHandlerFeature
      = HttpContext.Features.Get<IExceptionHandlerFeature>();
    var exception = exceptionHandlerFeature?.Error;

    // Check if the exception is a DbUpdateException with a foreign key violation
    if (exception is DbUpdateException dbUpdateException &&
        IsForeignKeyConstraintViolation(dbUpdateException))
    {
      // "Cannot delete this entity as it has related entities."
      ViewBag.ErrorMessage
        = "Cannot delete this item because it is being used in other parts of the application.";
      ViewBag.Code = 409;
    }
    else
    {
      // "An unexpected error occurred."
      ViewBag.ErrorMessage = "An unexpected error occurred.";
      ViewBag.Code = 500;
    }

    ViewBag.IsAuthenticated = IsAuthenticated;

    return View();
  }

  [Route("AccessDenied")]
  public IActionResult AccessDenied()
  {
    ViewBag.IsAuthenticated = IsAuthenticated;
    return View();
  }

  [Route("ErrorWithStatusCode/{code}")]
  public IActionResult ErrorWithStatusCode(int code)
  {
    if (ModelState.IsValid && code == 404)
    {
      ViewBag.Title = "Page Not Found";
      ViewBag.ErrorMessage = "The page you are looking for does not exist.";
    }
    else
    {
      ViewBag.Title = "An Error Occurred";
      ViewBag.ErrorMessage = "An unexpected error occurred.";
    }

    ViewBag.Code = code;
    ViewBag.IsAuthenticated = IsAuthenticated;
    return View();
  }
}
