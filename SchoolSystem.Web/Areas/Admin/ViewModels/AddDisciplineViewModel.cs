using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Web.Areas.Admin.ViewModels;

public class AddDisciplineViewModel
{
    [Required,Display(Name = "Discipline")]
    public string DisciplineId { get; set; }

    public List<SelectListItem>? Disciplines { get; set; }

    [Required, Display(Name = "Class")]
    public string ClassId { get; set; }

    public List<SelectListItem>? Classes { get; set; }
}