namespace SchoolSystem.Web.ViewModels.Home;

public class CourseViewModel
{
  public Guid Id { get; init; }
  public string Name { get; set; }
  public string CoverImageUrl { get; set; }

  public string Description { get; set; }
}
