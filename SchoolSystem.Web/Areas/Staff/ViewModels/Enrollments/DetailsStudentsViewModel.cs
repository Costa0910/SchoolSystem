namespace SchoolSystem.Web.Areas.Staff.ViewModels.Enrollments;

public class DetailsStudentsViewModel
{
  public Guid Id { get; init; }
  public string Name { get; set; }
  public string CoverImageUrl { get; set; }

  public string Description { get; set; }

  public List<Models.Student> Students { get; set; }
  public string StartDate { get; set; }
}
