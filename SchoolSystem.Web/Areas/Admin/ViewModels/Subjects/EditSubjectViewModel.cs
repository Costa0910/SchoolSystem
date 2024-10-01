using System.ComponentModel.DataAnnotations;

namespace SchoolSystem.Web.Areas.Admin.ViewModels.Subjects;

public class EditSubjectViewModel
{
  [Required]
  public Guid Id { get; set; }

  [Required, MaxLength(200)]
  public string Name { get; set; }

  [MaxLength(1000), Required]
  public string Description { get; set; }
}
