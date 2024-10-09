using System.ComponentModel.DataAnnotations;
using SchoolSystem.Web.Models;

namespace SchoolSystem.Web.Areas.Staff.ViewModels.Enrollments;

public class AddSubjectViewModel
{
  public string? Name { get; set; }
  public IEnumerable<Subject>? Subjects { get; set; }
  [Required] public string CourseId { get; set; }

  [Required(ErrorMessage = "Choose subject to add")]
  public string SubjectId { get; set; }
}
