using System.ComponentModel.DataAnnotations;

namespace Web.Areas.Admin.ViewModels;

public class DisciplineEditViewModel
{
    [Required]
    public Guid Id { get; set; }

    [Required, MaxLength(200)]
    public string Name { get; set; }

    [MaxLength(500), Required]
    public string Description { get; set; }
}