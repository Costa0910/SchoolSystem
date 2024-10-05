using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolSystem.Web.Areas.Staff.Helpers;
using SchoolSystem.Web.Areas.Staff.ViewModels.Grades;
using SchoolSystem.Web.Controllers;
using SchoolSystem.Web.Data.Interfaces;
using SchoolSystem.Web.Helpers.Interfaces;
using SchoolSystem.Web.Models;
using SchoolSystem.Web.Models.EnumsClasses;

namespace SchoolSystem.Web.Areas.Staff.Controllers;

[Area(Roles.Staff), Authorize(Roles = Roles.Staff)]
public class GradesController(
  IUserHelper userHelper,
  ICourseRepository courseRepository,
  IMapper mapper,
  IGradeRepository gradeRepository)
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

  public async Task<IActionResult> StudentsToGrade(Guid courseId)
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

  [HttpGet]
  public async Task<IActionResult> GetStudentGrades(string courseId, string
    studentId)
  {
    if (!ModelState.IsValid)
    {
      BadRequest();
    }

    var course
      = await courseRepository.GetCourseWithStudentsSubjectsAndGrades(
        Guid.Parse(courseId));
    if (course == null)
    {
      return NotFound();
    }

    var student = course.Students.Find(s => s.Id == Guid.Parse(studentId));
    if (student == null)
    {
      return NotFound();
    }

    var grades = course.Subjects.Select(s => new
    {
      s.Id,
      s.Name,
      Grade = GetGrades(course, student, s)
    });

    return Json(grades);
  }

  [HttpPost]
  public async Task<IActionResult> SaveGrade([FromBody] SaveGradeModel model)
  {
    if (!ModelState.IsValid)
    {
      return BadRequest();
    }

    var course
      = await courseRepository.GetCourseWithStudentsSubjectsAndGrades(
        Guid.Parse(model.CourseId));
    if (course == null)
    {
      return NotFound();
    }

    var student
      = course.Students.Find(s => s.Id == Guid.Parse(model.StudentId));
    if (student == null)
    {
      return NotFound();
    }

    var subject
      = course.Subjects.Find(s => s.Id == Guid.Parse(model.SubjectId));
    if (subject == null)
    {
      return NotFound();
    }

    var grades
      = course.Grades.Find(g => g.Student == student && g.Subject == subject);
    if (grades == null)
    {
      var newGrade = new Grade
      {
        Id = Guid.NewGuid(),
        Student = student,
        Subject = subject,
        Value = model.Grade,
        Status = model.Grade >= 10 ? StatusGrade.Pass : StatusGrade.Fail
      };

      await gradeRepository.AddAsync(newGrade);
      course.Grades.Add(newGrade);
      await courseRepository.UpdateAsync(course);
    }
    else
    {
      grades.Value = model.Grade;
      grades.Status = model.Grade >= 10 ? StatusGrade.Pass : StatusGrade.Fail;
      await gradeRepository.UpdateAsync(grades);
    }
    return Ok();
  }

  /// <summary>
  ///  Get the grades of a student in a course
  /// </summary>
  /// <param name="course">A where to search for grade</param>
  /// <param name="student">Grade of this student</param>
  /// <param name="subject">Subject where student got the grade</param>
  /// <returns></returns>
  private double GetGrades(Course course, Models.Student student,
    Subject subject)
  {
    var grades = course.Grades.Find(g => g.Student == student &&
                                         g.Subject == subject);
    if (grades == null)
    {
      return 0;
    }

    return grades.Value;
  }
}
