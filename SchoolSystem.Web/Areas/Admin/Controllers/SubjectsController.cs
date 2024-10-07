using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolSystem.Web.Areas.Admin.ViewModels.Subjects;
using SchoolSystem.Web.Controllers;
using SchoolSystem.Web.Data.Interfaces;
using SchoolSystem.Web.Helpers.Interfaces;
using SchoolSystem.Web.Models;
using SchoolSystem.Web.Models.EnumsClasses;

namespace SchoolSystem.Web.Areas.Admin.Controllers;

[Area(Roles.Admin), Authorize(Roles = Roles.Admin)]
public class SubjectsController(
  ISubjectRepository subjectRepository, IUserHelper userHelper)
  : BaseController(userHelper)
{
  public async Task<IActionResult> Index(string? message)
  {
    if (!string.IsNullOrWhiteSpace(message))
      ViewBag.Message = message;
    var subjects = await subjectRepository.GetAllAsync();
    return View(subjects);
  }

  public async Task<IActionResult> Edit(string id)
  {
    if (!ModelState.IsValid)
      return RedirectToAction(nameof(Index),
        new { message = "Subject not found" });

    var subject = await subjectRepository.GetByIdAsync(Guid.Parse(id));

    if (subject is null)
      return RedirectToAction(nameof(Index),
        new { message = "Subject not found." });

    var model = new EditSubjectViewModel
    {
      Id = subject.Id,
      Name = subject.Name,
      Description = subject.Description
    };

    return View(model);
  }

  [HttpPost, ValidateAntiForgeryToken]
  public async Task<IActionResult> Edit(EditSubjectViewModel model)
  {
    if (!ModelState.IsValid)
      return View(model);

    var subject = await subjectRepository.GetByIdAsync(model.Id);

    if (subject is null)
    {
      return RedirectToAction(nameof(Index),
        new { message = "Subject not found." });
    }

    subject.Name = model.Name;
    subject.Description = model.Description;

    try
    {
      await subjectRepository.UpdateAsync(subject);
    }
    catch (Exception e)
    {
      if (e.InnerException is not null &&
          e.InnerException.Message.Contains("duplicate key"))
        ViewBag.Error = "subject already exists";
      else
        ViewBag.Error = "An error occurred while updating the subject";

      return View(model);
    }

    return RedirectToAction(nameof(Index),
      new { message = $"{subject.Name} updated successfully" });
  }

  public IActionResult Create()
  {
    return View();
  }

  [HttpPost, ValidateAntiForgeryToken]
  public async Task<IActionResult> Create(CreateSubjectViewModel model)
  {
    if (!ModelState.IsValid)
      return View(model);

    var subject = new Subject
    {
      Name = model.Name,
      Description = model.Description,
      Hours = model.Hours,
      Courses = []
    };

    try
    {
      await subjectRepository.AddAsync(subject);
    }
    catch (Exception e)
    {
      if (e.InnerException is not null &&
          e.InnerException.Message.Contains("duplicate key"))
        ViewBag.Error = $"{subject.Name} already exists";
      else
        ViewBag.Error = "An error occurred while creating the Subject";

      return View(model);
    }

    return RedirectToAction(nameof(Index),
      new { message = $"{subject.Name} created successfully" });
  }

  public async Task<IActionResult> Delete(string id)
  {
    if (!ModelState.IsValid)
      return RedirectToAction(nameof(Index),
        new { message = "Subject not found." });

    var subject = await subjectRepository.GetByIdAsync(Guid.Parse(id));

    if (subject is null)
      return RedirectToAction(nameof(Index),
        new { message = "Subject not found." });

    try
    {
      await subjectRepository.DeleteAsync(subject);
    }
    catch (Exception)
    {
      //Tdod: check if the discipline is used in any other entity
      return RedirectToAction(nameof(Index),
        new { message = "An error occurred while deleting the subject" });
    }

    return RedirectToAction(nameof(Index),
      new { message = $"{subject.Name} deleted successfully" });
  }
}
