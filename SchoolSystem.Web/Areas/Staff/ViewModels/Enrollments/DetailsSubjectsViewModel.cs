using SchoolSystem.Web.Models;

namespace SchoolSystem.Web.Areas.Staff.ViewModels.Enrollments;

public class DetailsSubjectsViewModel
{
  public Guid Id { get; init; }
  public string Name { get; set; }
  public string CoverImageUrl { get; set; }

  public string Description { get; set; }

  public List<Subject> Subjects { get; set; }
  public string StartDate { get; set; }
}
