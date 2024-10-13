using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
  IMapper mapper,
  ISubjectRepository subjectRepository)
  : BaseController(userHelper)
{
  public async Task<IActionResult> Index(string? message)
  {
    if (!string.IsNullOrEmpty(message))
      ViewBag.Message = message;
    var courses = await courseRepository.GetCoursesWithSubjectsAndStudents();

    return View(courses);
  }

  public async Task<IActionResult> AddStudent(string courseId)
  {
    if (!ModelState.IsValid)
    {
      return RedirectToAction(nameof(Index),
        new { message = "Course not found" });
    }

    var course = await courseRepository.GetByIdAsync(Guid.Parse(courseId));

    if (course == null)
      return RedirectToAction(nameof(Index),
        new { message = "Course not found" });

    var students = await studentRepository.GetStudentsNotInCourseAsync(course);

    var viewModel = new EnrollStudentViewModel
    {
      Students = students,
      Name = course.Name,
    };

    return View(viewModel);
  }

  [HttpPost, ValidateAntiForgeryToken]
  public async Task<IActionResult> AddStudent(EnrollStudentViewModel model)
  {
    if (!ModelState.IsValid)
    {
      ViewBag.Error = "Invalid data, please try again";
      var courseModel
        = await courseRepository.GetByIdAsync(Guid.Parse(model.CourseId));

      if (courseModel == null)
        return RedirectToAction(nameof(Index),
          new { message = "Course not found" });

      var students
        = await studentRepository.GetStudentsNotInCourseAsync(courseModel);
      model.Students = students;
      model.Name = courseModel.Name;

      return View(model);
    }

    var course = await courseRepository.GetCourseWithStudents(Guid.Parse
      (model.CourseId));
    var student
      = await studentRepository.GetStudentByIdIncludeUserAsync(
        Guid.Parse(model.StudentId));

    if (course == null || student == null)
    {
      return RedirectToAction(nameof(Index),
        new { message = "Course or student not found" });
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

  public async Task<IActionResult> AddSubject(string courseId)
  {
    if (!ModelState.IsValid)
    {
      return RedirectToAction(nameof(Index),
        new { message = "Course not found" });
    }

    var course
      = await courseRepository.GetCourseWithSubjects(Guid.Parse(courseId));

    if (course == null)
      return RedirectToAction(nameof(Index),
        new { message = "Course not found" });

    var subjects = await subjectRepository.GetSubjectsNotInCourseAsync(course);

    var viewModel = new AddSubjectViewModel
    {
      Subjects = subjects,
      Name = course.Name,
    };

    return View(viewModel);
  }

  [HttpPost, ValidateAntiForgeryToken]
  public async Task<IActionResult> AddSubject(AddSubjectViewModel model)
  {
    if (!ModelState.IsValid)
    {
      ViewBag.Error = "Invalid data, please try again";
      var courseModel
        = await courseRepository.GetCourseWithSubjects(
          Guid.Parse(model.CourseId));

      if (courseModel == null)
        return RedirectToAction(nameof(Index),
          new { message = "Course not found" });

      var subjects
        = await subjectRepository.GetSubjectsNotInCourseAsync(courseModel);
      model.Subjects = subjects;
      model.Name = courseModel.Name;

      return View(model);
    }

    var course = await courseRepository.GetCourseWithSubjects(Guid.Parse
      (model.CourseId));
    var subject
      = await subjectRepository.GetByIdAsync(Guid.Parse(model.SubjectId));

    if (course == null || subject == null)
    {
      return RedirectToAction(nameof(Index),
        new { message = "Course or subject not found" });
    }

    course.Subjects.Add(subject);
    await courseRepository.UpdateAsync(course);

    return RedirectToAction(nameof(Index),
      new
      {
        message
          = $"Subject {subject.Name} added to course {course.Name}"
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

  public async Task<IActionResult> DetailsSubjects(string id, string? message)
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

    var course = await courseRepository.GetCourseWithSubjects(Guid.Parse
      (id));

    if (course is null)
    {
      return RedirectToAction(nameof(Index),
        new { message = "course not found." });
    }

    var model = mapper.Map<DetailsSubjectsViewModel>(course);

    return View(model);
  }

  public async Task<IActionResult> DetailsStudents(string id, string? message)
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

    var model = mapper.Map<DetailsStudentsViewModel>(course);

    return View(model);
  }

  public async Task<IActionResult> DeleteSubject(string courseId,
    string id)
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
        return RedirectToAction(nameof(DetailsSubjects),
          new { id = courseId, message = "Course or subject not found" });
      }
    }

    var course = await courseRepository.GetCourseWithSubjects(Guid.Parse
      (courseId));
    var subject
      = await subjectRepository.GetByIdAsync(Guid.Parse(id));

    if (course == null || subject == null)
    {
      return RedirectToAction(nameof(Index),
        new { message = "Course or subject not found" });
    }

    var canDelete
      = await subjectRepository.CanDeleteSubjectAsync(subject, course.Id);
    if (canDelete)
    {
      course.Subjects.Remove(subject);
      await courseRepository.UpdateAsync(course);
    }
    else
    {
      throw new DbUpdateException(
        "DELETE statement conflicted with the REFERENCE constraint",
        new Exception(
          "DELETE statement conflicted with the REFERENCE constraint"));
    }

    return RedirectToAction(nameof(DetailsSubjects),
      new
      {
        id = courseId,
        message
          = $"Subject {subject.Name} removed from Course {course.Name}"
      });
  }

  public async Task<IActionResult> DeleteStudent(string courseId,
    string id)
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
        return RedirectToAction(nameof(DetailsStudents),
          new { id = courseId, message = "Course or student not found" });
      }
    }

    var course = await courseRepository.GetCourseWithStudents(Guid.Parse
      (courseId));
    var student
      = await studentRepository.GetStudentByIdIncludeUserAsync(
        Guid.Parse(id));

    if (course == null || student == null)
    {
      return RedirectToAction(nameof(Index),
        new { message = "Course or student not found" });
    }

    var canDelete
      = await studentRepository.CanDeleteStudentAsync(student, course.Id);
    if (canDelete)
    {
      course.Students.Remove(student);
      await courseRepository.UpdateAsync(course);
    }
    else
    {
      throw new DbUpdateException(
        "DELETE statement conflicted with the REFERENCE constraint",
        new Exception(
          "DELETE statement conflicted with the REFERENCE constraint"));
    }

    return RedirectToAction(nameof(DetailsStudents),
      new
      {
        id = courseId,
        message
          = $"Student {student.User.FirstName} {student.User.LastName} removed from Course {course.Name}"
      });
  }
}
