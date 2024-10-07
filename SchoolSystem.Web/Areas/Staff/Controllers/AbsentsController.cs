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
public class AbsentsController(
  IUserHelper userHelper,
  ICourseRepository
    courseRepository,
  IMapper mapper,
  IAttendanceRepository attendanceRepository)
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

  public async Task<IActionResult> StudentsToAddAbsent(Guid courseId)
  {
    if (!ModelState.IsValid)
    {
      return RedirectToAction(nameof(Index),
        new { message = "Invalid course id" });
    }

    var course
      = await courseRepository
        .GetCourseWithStudentsDetailsAndSubjects(courseId);
    if (course == null)
    {
      return RedirectToAction(nameof(Index),
        new { message = "Course not found" });
    }

    ViewBag.Ex = course.Subjects[0]?.Name ?? "No subjects";
    ViewBag.Subjects
      = course.Subjects.Select(s => new { s.Name, s.Id }).ToList();

    return View(course);
  }

  [HttpPost]
  public async Task<IActionResult> MarkAbsent([FromBody]MarkAbsentModel model)
  {
    if (!ModelState.IsValid)
    {
      return BadRequest();
    }

    var course = await courseRepository.GetCourseWithStudentsSubjectsAndAbsent(
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

    var absent = new Attendance
    {
      Subject = subject,
      Student = student,
      Status = StatusAttendance.Absent,
      Date = DateTime.Now,
      Id = Guid.NewGuid()
    };

    await attendanceRepository.AddAsync(absent);
    course.Attendances.Add(absent);
    await courseRepository.UpdateAsync(course);
    return Ok();
  }
}
