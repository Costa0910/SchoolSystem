using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace SchoolSystem.Web.Areas.Admin.ViewModels.Courses;

public class CreateCourseViewModel
{
  [Required, MaxLength(200)] public string Name { get; set; }

  [Required, MaxLength(1000)] public string Description { get; set; }

  [Display(Name = "Cover Image")] public IFormFile? CoverImage { get; set; }


  [Required, Display(Name = "Start Date"), DataType(DataType.Date,
     ErrorMessage = "Invalid date format")]
  public DateOnly StartDate { get; set; }

  [Range(5, 40, ErrorMessage = "Value must be between {0} and {1}"),
   Required, Display(Name = "Exclusion Percentage")]
  public double ExclusionPercentage { get; set; }
}
