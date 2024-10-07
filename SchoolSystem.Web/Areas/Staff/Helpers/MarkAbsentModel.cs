using System.ComponentModel.DataAnnotations;

namespace SchoolSystem.Web.Areas.Staff.Helpers;

public class MarkAbsentModel
{
  [Required] public string StudentId { get; set; }
  [Required] public string CourseId { get; set; }
  [Required] public string SubjectId { get; set; }
}
