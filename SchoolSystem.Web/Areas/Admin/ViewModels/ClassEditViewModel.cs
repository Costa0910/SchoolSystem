using System.ComponentModel.DataAnnotations;

namespace Web.Areas.Admin.ViewModels;

public class ClassEditViewModel
{
    public string Id { get; set; }

    [Required, MaxLength(500)]
    public string Name { get; set; }

    [Required]
    public string Description { get; set; }

    [Display(Name = "Class Image")]
    public IFormFile? ClassImage { get; set; }

    [Required, Display(Name = "Start Date")]
    public DateOnly StartDate { get; set; }

    [Required, Display(Name = "End Date")]
    public DateOnly EndDate { get; set; }
}