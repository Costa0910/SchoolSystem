using AutoMapper;
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
  IStudentRepository studentRepository,
  IMapper mapper)
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
    var student
      = await studentRepository.GetStudentByIdIncludeUserAsync(
        Guid.Parse(viewModel.StudentId));

    if (course == null || student == null)
    {
      ViewBag.ErrorMessage = "Course or student not found";
      viewModel.Courses = await courseRepository.GetAllAsync();
      return View(viewModel);
    }

    course.Students.Add(student);
    await courseRepository.UpdateAsync(course);

    return RedirectToAction(nameof(Index),
      new
      {
        message
          = $"Student {student.User.FirstName} {student.User.LastName} enrolled in course {course.Name}"
      });
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

  public async Task<IActionResult> Details(string id, string? message)
  {
    if (!string.IsNullOrEmpty(message))
    {
      ViewBag.Message = message;
    }

    if (!ModelState.IsValid)
    {
      return RedirectToAction(nameof(Index),
        new { message = "Course not found" });
    }

    var course = await courseRepository.GetCourseWithStudentsDetails(Guid.Parse
      (id));

    if (course is null)
    {
      return RedirectToAction(nameof(Index),
        new { message = "course not found." });
    }

    var model = mapper.Map<DetailsEnrollmentsViewModel>(course);

    return View(model);
  }

  public async Task<IActionResult> DeleteStudent(string courseId,
    string studentId)
  {
    if (!ModelState.IsValid)
    {
      if (string.IsNullOrEmpty(courseId))
      {
        return RedirectToAction(nameof(Index),
          new { message = "Course not found" });
      }
      else
      {
        return RedirectToAction(nameof(Details),
          new { id = courseId, message = "Course or student not found" });
      }
    }

    var course = await courseRepository.GetCourseWithStudents(Guid.Parse
      (courseId));
    var student
      = await studentRepository.GetStudentByIdIncludeUserAsync(
        Guid.Parse(studentId));

    if (course == null || student == null)
    {
      return RedirectToAction(nameof(Index),
        new { message = "Course or student not found" });
    }

    //TODO: found out if student been use in other places on this course before removing
    course.Students.Remove(student);
    await courseRepository.UpdateAsync(course);

    return RedirectToAction(nameof(Details),
      new
      {
        id = courseId,
        message
          = $"Student {student.User.FirstName} {student.User.LastName} removed from Course {course.Name}"
      });
  }
}
