using System.ComponentModel.DataAnnotations;

namespace SchoolSystem.Web.Areas.Staff.Helpers;

public class SaveGradeModel
{
  [Required] public string CourseId { get; init; }
  [Required] public string StudentId { get; init; }
  [Required] public string SubjectId { get; init; }
  [Required, Range(1, 20)] public double Grade { get; init; }
}
