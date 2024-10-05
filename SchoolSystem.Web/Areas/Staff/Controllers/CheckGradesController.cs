using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolSystem.Web.Areas.Staff.ViewModels.Grades;
using SchoolSystem.Web.Controllers;
using SchoolSystem.Web.Data.Interfaces;
using SchoolSystem.Web.Helpers.Interfaces;
using SchoolSystem.Web.Models;
using SchoolSystem.Web.Models.EnumsClasses;

namespace SchoolSystem.Web.Areas.Staff.Controllers;

[Area(Roles.Staff), Authorize(Roles = Roles.Staff)]
public class CheckGradesController(
  IUserHelper userHelper,
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

    var courses = await courseRepository.FindAsync(c => c.Subjects.Count != 0 &&
      c
        .Students.Count != 0);

    var model = mapper.Map<IEnumerable<CourseViewModel>>(courses);
    return View(model);
  }

  public async Task<IActionResult> StudentsToCheck(Guid courseId)
  {
    if (!ModelState.IsValid)
    {
      return RedirectToAction(nameof(Index),
        new { message = "Invalid course id" });
    }

    var course = await courseRepository.GetCourseWithStudentsDetails(courseId);
    if (course == null)
    {
      return RedirectToAction(nameof(Index),
        new { message = "Course not found" });
    }

    return View(course);
  }

  public async Task<IActionResult> GetStudentGrades(string courseId,
    string studentId)
  {
    if (!ModelState.IsValid)
    {
      return BadRequest();
    }

    var course = await courseRepository.GetCourseWithGradesAndStudents(Guid
      .Parse(courseId));

    if (course == null)
    {
      return NotFound();
    }

    var student = course.Students.Find(s => s.Id == Guid.Parse
      (studentId));

    if (student == null)
    {
      return NotFound();
    }

    var studentGrades = course.Grades.Where(g => g.Student == student);

    var dataToSend = await GetSubjectIfNoGrades(studentGrades, course.Id);
    return Json(dataToSend);
  }

  private async Task<object> GetSubjectIfNoGrades(IEnumerable<Grade>
    studentGrades, Guid courseId)
  {
    var subjectCourse
      = await courseRepository.GetCourseWithSubjects(courseId);
    if (subjectCourse == null)
    {
      return NotFound();
    }

    var enumerable = studentGrades.ToList();
    if (enumerable.Count == 0)
    {
      var subjects = subjectCourse.Subjects.Select(s => new
      {
        s.Id,
        s.Name,
        Grade = "No grade yet"
      });

      return subjects.OrderBy(s => s.Name);
    }

    var grades = subjectCourse.Subjects.Select(s => new
    {
      s.Id,
      s.Name,
      Grade = enumerable.Find(g => g.Subject == s)?.Value
    });

    return grades.OrderBy(s => s.Name);
  }
}
