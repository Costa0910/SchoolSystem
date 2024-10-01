using System.ComponentModel.DataAnnotations;

namespace SchoolSystem.Web.Areas.Admin.ViewModels.Subjects;

public class CreateSubjectViewModel
{
  [MaxLength(200), Required] public string Name { get; set; }

  [MaxLength(1000), Required] public string Description { get; set; }
}
