using SchoolSystem.Web.Models;

namespace SchoolSystem.Web.Areas.Admin.ViewModels.Courses;

public class CourseViewModel
{
  public Guid Id { get; init; }
  public string Name { get; set; }
  public string CoverImageUrl { get; set; }

  public string Description { get; set; }

  public List<Subject> Subjects { get; set; }
  public DateOnly StartDate { get; set; }
}
