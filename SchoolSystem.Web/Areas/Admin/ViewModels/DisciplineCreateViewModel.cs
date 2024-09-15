using System.ComponentModel.DataAnnotations;

namespace Web.Areas.Admin.ViewModels;

public class DisciplineCreateViewModel
{
    [MaxLength(200), Required]
    public string Name { get; set; }

    [MaxLength(500), Required]
    public string? Description { get; set; }
}