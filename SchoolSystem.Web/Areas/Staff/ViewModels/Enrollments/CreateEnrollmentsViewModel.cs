using System.ComponentModel.DataAnnotations;
using SchoolSystem.Web.Models;

namespace SchoolSystem.Web.Areas.Staff.ViewModels.Enrollments;

public class CreateEnrollmentsViewModel
{
  public IEnumerable<Course>? Courses { get; set; }
  public IEnumerable<Models.Student>? Students { get; set; }

  [Required(ErrorMessage = "Choose a course where to enroll the student")]
  public string CourseId { get; set; }

  [Required(ErrorMessage = "Choose a student to enroll")]
  public string StudentId { get; set; }
}