using System.Diagnostics;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolSystem.Web.Data.Interfaces;
using SchoolSystem.Web.Models;
using SchoolSystem.Web.ViewModels.Home;

namespace SchoolSystem.Web.Controllers;

[AllowAnonymous]
public class HomeController(ICourseRepository courseRepository, IMapper mapper)
  :
    Controller
{
  public IActionResult Index()
  {
    return View();
  }

  public IActionResult Privacy()
  {
    return View();
  }

  public async Task<IActionResult> Courses()
  {
    var courses = await courseRepository.GetAllAsync();
    var courseViewModel = mapper.Map<IEnumerable<CourseViewModel>>(courses);
    return View(courseViewModel.OrderBy(c => c.Name));
  }

  public async Task<IActionResult> CourseDetails(string id)
  {
    if (!ModelState.IsValid)
    {
      return NotFound();
    }

    var course = await courseRepository.GetCourseWithSubjects(Guid.Parse(id));
    if (course == null)
    {
      return NotFound();
    }
    return View(course);
  }

  public IActionResult About()
  {
    return View();
  }

  [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None,
    NoStore = true)]
  public IActionResult Error()
  {
    return View(new ErrorViewModel
    {
      RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
    });
  }
}
