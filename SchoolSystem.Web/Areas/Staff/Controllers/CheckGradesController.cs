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
public class CheckGradesController(
  IUserHelper userHelper,
  IMapper mapper,
  ICourseRepository courseRepository,
  IStudentRepository studentRepository)
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
      return BadRequest();

    var course
      = await courseRepository.GetCourseWithStudentsSubjectsAndGrades(Guid
        .Parse(courseId));

    if (course == null || course.Students.Count == 0)
      return NotFound();

    var student = await studentRepository.GetByIdAsync(Guid.Parse(studentId));
    if (course.Students.All(s => s != student))
      return NotFound();

    var grades = new List<ViewStudentGradeModel>();
    double totalGrades = 0;
    foreach (var subject in course.Subjects.OrderBy(s => s.Name))
    {
      var grade = course.Grades.Find(g => g.Subject == subject &&
                                          g.Student == student);

      if (grade == null)
      {
        grades.Add(new ViewStudentGradeModel
        {
          SubjectName = subject.Name,
          ExpectedHours = subject.Hours,
          Grade = 0,
          Status = "Not graded yet"
        });
      }
      else
      {
        grades.Add(new ViewStudentGradeModel
        {
          SubjectName = subject.Name,
          Grade = grade.Value,
          ExpectedHours = subject.Hours,
          Status = grade.Status
        });
        totalGrades += grade.Value;
      }
    }

    var absents = await AddAbsents(course, student, grades);
    var totalHours = AddAttendancePercentage(grades, course
      .ExclusionPercentage);
    var totalAttendancePercentage
      = CalculateAttendancePercentage(totalHours, absents);

    string totalStatus;

    if (grades.All(m => m.Status == StatusGrade.Pass))
    {
      totalStatus = StatusGrade.Pass;
    }
    else if (grades.Any(m => m.Status == StatusGrade.Fail))
    {
      totalStatus = StatusGrade.Fail;
    }
    else
    {
      totalStatus = "Not graded yet";
    }


    return Ok(new
    {
      grades,
      TotalAbsents = absents,
      TotalExpectedHours = totalHours,
      IsTotalExcluded = grades.Any(g => g.IsExcluded),
      TotalAttendancePercentage = Math.Round(totalAttendancePercentage, 2),
      Average = totalGrades > 0
        ? Math.Round(totalGrades / course.Subjects.Count, 2)
        : 0,
      TotalStatus = totalStatus
    });
  }


  private async Task<int> AddAbsents(Course course, Models.Student student,
    List<ViewStudentGradeModel> model)
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

  private int AddAttendancePercentage(List<ViewStudentGradeModel> model,
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
