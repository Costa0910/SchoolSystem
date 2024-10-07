using System.ComponentModel.DataAnnotations;
using SchoolSystem.Web.Data.Interfaces;

namespace SchoolSystem.Web.Models;

/// <summary>
/// Represents a course that a student can attend, with subjects, grades, attendances, etc.
/// </summary>
public class Course : IEntity
{
  public Guid Id { get; init; }
  [MaxLength(200)] public string Name { get; set; }
  public Guid? CoverImageId { get; set; }

  [MaxLength(1000)] public string Description { get; set; }
  public AdminUser CreatedBy { get; init; }

  public List<Subject> Subjects { get; set; }
  public List<Student> Students { get; set; }
  public List<Grade> Grades { get; set; }
  public List<Attendance> Attendances { get; set; }

  [Range(5, 40)] public double ExclusionPercentage { get; set; }
  public DateOnly StartDate { get; set; }
}
