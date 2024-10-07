using System.ComponentModel.DataAnnotations;
using SchoolSystem.Web.Data.Interfaces;

namespace SchoolSystem.Web.Models;

public class Subject : IEntity
{
  public Guid Id { get; init; }
  [MaxLength(200)] public string Name { get; set; }
  [MaxLength(1000)] public string Description { get; set; }

  [Range(10, 50)] public int Hours { get; set; }
  public List<Course> Courses { get; set; }
}
