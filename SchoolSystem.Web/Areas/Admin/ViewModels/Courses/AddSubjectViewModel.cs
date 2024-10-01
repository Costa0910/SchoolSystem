using System.ComponentModel.DataAnnotations;
using SchoolSystem.Web.Models;

namespace SchoolSystem.Web.Areas.Admin.ViewModels.Courses;

public class AddSubjectViewModel
{
  [Required(ErrorMessage = "Something went wrong. Please try again.")]
  public Guid Id { get; set; }

  public string? Name { get; set; }
  public List<Subject>? Subjects { get; set; }

  [Required(ErrorMessage = "Please select a subject")]
  public string SelectedSubjectId { get; set; }
}
