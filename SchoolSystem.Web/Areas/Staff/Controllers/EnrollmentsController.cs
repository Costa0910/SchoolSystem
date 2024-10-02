using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolSystem.Web.Areas.Staff.ViewModels.Enrollments;
using SchoolSystem.Web.Controllers;
using SchoolSystem.Web.Data.Interfaces;
using SchoolSystem.Web.Helpers.Interfaces;
using SchoolSystem.Web.Models.EnumsClasses;

namespace SchoolSystem.Web.Areas.Staff.Controllers;

[Area(Roles.Staff), Authorize(Roles = Roles.Staff)]
public class EnrollmentsController(
  IUserHelper userHelper,
  ICourseRepository courseRepository,
  IStudentRepository studentRepository)
  : BaseController(userHelper)
{
  public async Task<IActionResult> Index(string? message)
  {
    if (!string.IsNullOrEmpty(message))
      ViewBag.Message = message;
    var courses = await courseRepository.GetCoursesWithSubjectsAndStudents();

    return View(courses);
  }

  public async Task<IActionResult> Create()
  {
    var viewModel = new CreateEnrollmentsViewModel
    {
      Courses = await courseRepository.GetAllAsync(),
      Students = []
    };

    return View(viewModel);
  }

  [HttpPost, ValidateAntiForgeryToken]
  public async Task<IActionResult> Create(CreateEnrollmentsViewModel viewModel)
  {
    if (!ModelState.IsValid)
    {
      ViewBag.Error = "Invalid data";
      viewModel.Courses = await courseRepository.GetAllAsync();
      return View(viewModel);
    }

    var course = await courseRepository.GetCourseWithStudents(Guid.Parse
      (viewModel.CourseId));
    var student = await studentRepository.GetStudentByIdIncludeUserAsync(Guid.Parse(viewModel.StudentId));

    if (course == null || student == null)
    {
      ViewBag.ErrorMessage = "Course or student not found";
      viewModel.Courses = await courseRepository.GetAllAsync();
      return View(viewModel);
    }

    course.Students.Add(student);
    await courseRepository.UpdateAsync(course);

    return RedirectToAction(nameof(Index), new {message = $"Student {student.User.FirstName} {student.User.LastName} enrolled in course {course.Name}"});
  }

  [HttpGet]
  public async Task<IActionResult> GeAvailableStudents(string courseId)
  {
    if (!ModelState.IsValid)
      return BadRequest();

    var course = await courseRepository.GetByIdAsync(Guid.Parse(courseId));

    if (course == null)
      return NotFound();

    var students = await studentRepository.GetStudentsNotInCourseAsync(course);

    return Json(students.Select(s => new
    {
      id = s.Id,
      name = $"{s.User.FirstName} {s.User.LastName}"
    }));
  }
}
