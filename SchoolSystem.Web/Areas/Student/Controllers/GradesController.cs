using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolSystem.Web.Areas.Student.ViewModels.Grades;
using SchoolSystem.Web.Controllers;
using SchoolSystem.Web.Data.Interfaces;
using SchoolSystem.Web.Helpers.Interfaces;
using SchoolSystem.Web.Models;
using SchoolSystem.Web.Models.EnumsClasses;

namespace SchoolSystem.Web.Areas.Student.Controllers;

[Area(Roles.Student), Authorize(Roles = Roles.Student)]
public class GradesController(
  IUserHelper userHelper,
  IStudentRepository studentRepository,
  IMapper mapper,
  ICourseRepository courseRepository,
  IAttendanceRepository attendanceRepository)
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
          ExpectedHours = subject.Hours,
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
          ExpectedHours = subject.Hours,
          Status = grade.Status
        });
        total += grade.Value;
      }
    }

    if (total > 0)
    {
      ViewBag.Average = Math.Round(total / course.Subjects.Count, 2);
    }
    else
    {
      ViewBag.Average = 0;
    }

    var absents = await AddAbsents(course, student, model);
    var totalHours = AddAttendancePercentage(model, course.ExclusionPercentage);
    var totalAttendancePercentage
      = CalculateAttendancePercentage(totalHours, absents);

    if (model.All(m => m.Status == StatusGrade.Pass))
    {
      ViewBag.Status = StatusGrade.Pass;
    }
    else if (model.Any(m => m.Status == StatusGrade.Fail))
    {
      ViewBag.Status = StatusGrade.Fail;
    }
    else
    {
      ViewBag.Status = "Not graded yet";
    }


    ViewBag.CourseName = course.Name;
    ViewBag.Absents = absents;
    ViewBag.TotalHours = totalHours;
    ViewBag.IsExcluded = model.Any(m => m.IsExcluded);
    ViewBag.TotalAttendancePercentage
      = Math.Round(totalAttendancePercentage, 2);
    return View(model);
  }


  private async Task<int> AddAbsents(Course course, Models.Student student,
    List<ViewGradeViewModel> model)
  {
    int absents = 0;
    var foundCourse
      = await courseRepository.GetAbsentsAsync(course.Id, student);
    if (foundCourse == null)
    {
      return absents;
    }

    foreach (var absent in foundCourse.Attendances)
    {
      var subject = course.Subjects.Find(s => s == absent.Subject);
      if (subject == null) continue;
      var grade = model.Find(g => g.SubjectName == subject.Name);
      if (grade != null)
      {
        grade.Absents += 4; // 4 hours of class
        absents += 4;
      }
    }

    return absents;
  }

  private int AddAttendancePercentage(List<ViewGradeViewModel> model,
    double exclusionPercentage)
  {
    int totalHours = 0;
    foreach (var grade in model)
    {
      totalHours += grade.ExpectedHours;
      var attendancePercentage = CalculateAttendancePercentage(
        grade.ExpectedHours, grade.Absents);
      grade.AttendancePercentage = Math.Round(attendancePercentage, 2);
      grade.IsExcluded = IsExcluded(grade.ExpectedHours, grade.Absents,
        exclusionPercentage);
    }

    return totalHours;
  }


  private double CalculateAttendancePercentage(int totalHours, int absents)
  {
    return ((totalHours - absents) / (double)totalHours) * 100;
  }

  private bool IsExcluded(int totalHours, int absents,
    double allowedAbsentPercentage)
  {
    double attendancePercentage
      = CalculateAttendancePercentage(totalHours, absents);
    return attendancePercentage < (100 - allowedAbsentPercentage);
  }
}
