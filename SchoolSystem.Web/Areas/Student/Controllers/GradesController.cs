using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolSystem.Web.Areas.Student.ViewModels.Grades;
using SchoolSystem.Web.Controllers;
using SchoolSystem.Web.Data.Interfaces;
using SchoolSystem.Web.Helpers.Interfaces;
using SchoolSystem.Web.Models.EnumsClasses;

namespace SchoolSystem.Web.Areas.Student.Controllers;

[Area(Roles.Student), Authorize(Roles = Roles.Student)]
public class GradesController(
  IUserHelper userHelper,
  IStudentRepository studentRepository,
  IMapper mapper,
  ICourseRepository courseRepository)
  : BaseController(userHelper)
{
  public async Task<IActionResult> Index(string? message)
  {
    if (!string.IsNullOrEmpty(message))
    {
      ViewBag.Message = message;
    }

    var student = await studentRepository.GetStudentCoursesByEmailAsync(User
      .Identity.Name);

    if (student == null)
    {
      return RedirectToAction("Login", "Auth", new { area = "" });
    }

    var myCourses = mapper.Map<List<MyCoursesViewModel>>(student.Courses);

    return View(myCourses);
  }

  public async Task<IActionResult> CheckMyGrades(Guid courseId)
  {
    if (!ModelState.IsValid)
    {
      return RedirectToAction("Index", new { message = "Invalid course id" });
    }
    var student = await studentRepository.GetStudentCoursesByEmailAsync(User
      .Identity.Name);

    if (student == null)
    {
      return RedirectToAction("Login", "Auth", new { area = "" });
    }

    var course
      = await courseRepository.GetCourseWithStudentsSubjectsAndGrades(courseId);

    if (course == null)
    {
      return RedirectToAction("Index", new { message = "Course not found" });
    }

    if (course.Subjects.Count == 0)
    {
      return RedirectToAction("Index",
        new { message = "Course has no subjects yet" });
    }


    var model = new List<ViewGradeViewModel>();

    double total = 0;
    foreach (var subject in course.Subjects.OrderBy(s => s.Name))
    {
      var grade = course.Grades.Find(g => g.Subject == subject &&
                                          g.Student == student);

      if (grade == null)
      {
        model.Add(new ViewGradeViewModel
        {
          SubjectName = subject.Name,
          Grade = 0,
          Status = "Not graded yet"
        });
      }
      else
      {
        model.Add(new ViewGradeViewModel
        {
          SubjectName = subject.Name,
          Grade = grade.Value,
          Status = grade.Status
        });
        total += grade.Value;
      }
    }

    if (total > 0)
    {
      ViewBag.Average = total / model.Count;
    }
    else
    {
      ViewBag.Average = 0;
    }

    ViewBag.Total = total;


    ViewBag.CourseName = course.Name;
    return View(model);
  }
}
