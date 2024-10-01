using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolSystem.Web.Areas.Admin.ViewModels.Courses;
using SchoolSystem.Web.Controllers;
using SchoolSystem.Web.Data.Interfaces;
using SchoolSystem.Web.Helpers.Interfaces;
using SchoolSystem.Web.Models;
using SchoolSystem.Web.Models.EnumsClasses;
using SchoolSystem.Web.Services.Interfaces;

namespace SchoolSystem.Web.Areas.Admin.Controllers;

[Area(Roles.Admin), Authorize(Roles = Roles.Admin)]
public class CoursesController(
  IBlobStorageService
    blobStorageService,
  IMapper mapper,
  ICourseRepository courseRepository,
  IAdminRepository adminRepository,
  ISubjectRepository subjectRepository, IUserHelper userHelper)
  : BaseController(userHelper)
{
//     // GET
  public async Task<IActionResult> Index(string? message)
  {
    if (!string.IsNullOrEmpty(message))
    {
      ViewBag.Message = message;
    }

    var courses = await courseRepository.GetCoursesWithSubjectsAndStudents();

    return View(courses);
  }

  public async Task<IActionResult> Edit(string id)
  {
    if (!ModelState.IsValid)
    {
      return RedirectToAction(nameof(Index),
        new { message = "Course not found" });
    }

    var course = await courseRepository.GetByIdAsync(Guid.Parse(id));

    if (course is null)
    {
      return RedirectToAction(nameof(Index),
        new { message = "Course not found." });
    }

    var model = mapper.Map<EditCourseViewModel>(course);

    return View(model);
  }


  [HttpPost, ValidateAntiForgeryToken]
  public async Task<IActionResult> Edit(EditCourseViewModel model)
  {
    if (!ModelState.IsValid)
    {
      return View(model);
    }

    if (model.StartDate < DateOnly.FromDateTime(DateTime.Now))
    {
      ViewBag.Error = "Start date must be in the future.";
      return View(model);
    }

    var course = await courseRepository.GetByIdAsync(model.Id);
    if (course is null)
    {
      return RedirectToAction(nameof(Index),
        new { message = "Course not found." });
    }

    var coverImageId = course.CoverImageId;

    if (model.CoverImage is not null && model.CoverImage.Length > 0)
    {
      if (course.CoverImageId != Guid.Empty && course.CoverImageId.HasValue)
      {
        await blobStorageService.DeleteFileAsync(course.CoverImageId.Value,
          AzureContainerNames.classes);
      }

      coverImageId = await blobStorageService.UploadFileAsync(model
        .CoverImage, AzureContainerNames.classes);
    }

    var updatedCourse = mapper.Map(model, course);
    updatedCourse.CoverImageId = coverImageId;

    try
    {
      await courseRepository.UpdateAsync(updatedCourse);
    }
    catch (Exception e)
    {
      ViewBag.Error = e.Message;

      return View(model);
    }

    return RedirectToAction(nameof(Index),
      new { message = $"{course.Name} updated successfully." });
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

    var course = await courseRepository.GetCourseWithSubjects(Guid.Parse
      (id));

    if (course is null)
    {
      return RedirectToAction(nameof(Index),
        new { message = "course not found." });
    }

    var model = mapper.Map<CourseViewModel>(course);

    return View(model);
  }

  public async Task<IActionResult> Delete(string id)
  {
    if (!ModelState.IsValid)
    {
      return RedirectToAction(nameof(Index),
        new { message = "Course not found" });
    }

    var course = await courseRepository.GetByIdAsync(Guid.Parse(id));

    if (course is null)
    {
      return RedirectToAction(nameof(Index),
        new { message = "Course not found." });
    }

    if (course.CoverImageId != Guid.Empty && course.CoverImageId.HasValue)
    {
      await blobStorageService.DeleteFileAsync(course.CoverImageId.Value,
        AzureContainerNames.classes);
    }


    try
    {
      await courseRepository.DeleteAsync(course);
    }
    catch (Exception e)
    {
      return RedirectToAction(nameof(Index), new { message = e.Message });
    }

    return RedirectToAction(nameof(Index),
      new { message = $"{course.Name} deleted successfully." });
  }

  [HttpGet]
  public IActionResult Create()
    => View();

  [HttpPost, ValidateAntiForgeryToken]
  public async Task<IActionResult> Create(CreateCourseViewModel model)
  {
    if (!ModelState.IsValid)
    {
      return View(model);
    }

    if (model.StartDate < DateOnly.FromDateTime(DateTime.Now))
    {
      ViewBag.Error = "Start date must be in the future.";
      return View(model);
    }

    var classImageId = Guid.Empty;

    if (model.CoverImage is not null && model.CoverImage.Length > 0)
    {
      classImageId = await blobStorageService.UploadFileAsync(model
        .CoverImage, AzureContainerNames.classes);
    }

    var admin = await adminRepository.GetAdminUserByUserEmailAsync(User
      .Identity
      .Name);
    if (admin is null)
    {
      ViewBag.Error = "You are not authorized to create a course.";
      return View(model);
    }

    var newCourse = new Course
    {
      Name = model.Name,
      Description = model.Description,
      StartDate = model.StartDate,
      CoverImageId = classImageId,
      Grades = [],
      Students = [],
      Subjects = [],
      Attendances = [],
      CreatedBy = admin
    };

    try
    {
      await courseRepository.AddAsync(newCourse);
      return RedirectToAction(nameof(Index),
        new { message = $"{newCourse.Name} created successfully." });
    }
    catch (Exception err)
    {
      if (classImageId != Guid.Empty)
      {
        await blobStorageService.DeleteFileAsync(classImageId,
          AzureContainerNames.classes);
      }

      if (err.InnerException is not null &&
          err.InnerException.Message.Contains("duplicate"))
      {
        ViewBag.Error = $"{newCourse.Name} already exists.";
      }
      else
      {
        ViewBag.Error = err.Message;
      }

      return View(model);
    }
  }

  /// <summary>
  /// Get available subjects for a course to add, based on the subjects that are not already added to the course.
  /// </summary>
  /// <param name="course">Course to add subjects</param>
  /// <returns>A list of subjects</returns>
  private async Task<List<Subject>> GetAvailableSubjects(Course course = null)
  {
    var subjects = await subjectRepository.GetAllAsync();

    if (course is null || !subjects.Any())
    {
      return new List<Subject>();
    }

    return subjects.Where(sub => !course.Subjects
        .Select(s => s.Id)
        .Contains(sub.Id))
      .ToList();
  }

  public async Task<IActionResult> AddSubject(string id)
  {
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
        new { message = "Course not found." });
    }

    var availableSubjects = await GetAvailableSubjects(course);

    var model = new AddSubjectViewModel
    {
      Id = course.Id,
      Name = course.Name,
      Subjects = availableSubjects
    };

    if (!availableSubjects.Any())
    {
      ViewBag.Error = "No subjects available.";
      return View(model);
    }

    return View(model);
  }

  [HttpPost, ValidateAntiForgeryToken]
  public async Task<IActionResult> AddSubject(AddSubjectViewModel model)
  {
    var course = await courseRepository.GetCourseWithSubjects(model.Id);
    if (course is null)
    {
      return RedirectToAction(nameof(Index),
        new { message = "Course not found." });
    }

    if (!ModelState.IsValid)
    {
      var availableSubjects = await GetAvailableSubjects(course);
      var newModel = new AddSubjectViewModel
      {
        Id = course.Id,
        Name = course.Name,
        Subjects = availableSubjects,
        SelectedSubjectId = model.SelectedSubjectId
      };
      return View(newModel);
    }

    var subject = await subjectRepository.GetByIdAsync(Guid.Parse
      (model.SelectedSubjectId));

    if (subject is null)
    {
      return RedirectToAction(nameof(Index),
        new { message = "Subject not found." });
    }

    if (course.Subjects.Exists(s => s.Id == subject.Id))
    {
      ViewBag.Error = $"{subject.Name} already exists in {course.Name}.";
      return View(model);
    }

    course.Subjects.Add(subject);

    try
    {
      await courseRepository.UpdateAsync(course);
    }
    catch (Exception)
    {
      return RedirectToAction(nameof(Details),
        new
        {
          id = course.Id,
          message = $"Could not add {subject.Name} to ${course.Name}"
        });
    }

    return RedirectToAction(nameof(Details),
      new
      {
        id = course.Id,
        message = $"{subject.Name} added to {course.Name} successfully."
      });
  }

//TODO: check if subject is not been used in some part of the system with this course before deleting
  public async Task<IActionResult> DeleteSubjectFromCourse(string courseId,
    string
      subjectId)
  {
    if (!ModelState.IsValid)
    {
      return RedirectToAction(nameof(Index),
        new { message = "Course not found" });
    }

    var course = await courseRepository.GetCourseWithSubjects(Guid.Parse
      (courseId));

    if (course is null)
    {
      return RedirectToAction(nameof(Index),
        new { message = "Course not found." });
    }

    var subject = course.Subjects.Find(s => s.Id == Guid.Parse(subjectId));
    if (subject is null)
    {
      return RedirectToAction(nameof(Details),
        new { id = courseId, message = "Subject not found." });
    }

    course.Subjects.Remove(subject);

    try
    {
      await courseRepository.UpdateAsync(course);
    }
    catch (Exception)
    {
      return RedirectToAction(nameof(Details),
        new
        {
          id = courseId,
          message = $"Could not delete {subject.Name} from {course.Name}"
        });
    }

    return RedirectToAction(nameof(Details),
      new
      {
        id = courseId,
        message = $"{subject.Name} deleted from {course.Name} successfully."
      });
  }
}
