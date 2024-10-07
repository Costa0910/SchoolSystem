using System.ComponentModel.DataAnnotations;
using SchoolSystem.Web.Data.Interfaces;

namespace SchoolSystem.Web.Models;

/// <summary>
/// Represents the attendance of a student in a class
/// </summary>
public class Attendance : IEntity
{
    public Guid Id { get; init; }
    public DateTime Date { get; set; }

    [MaxLength(50)]
    public string Status { get; set; } // Present, Absent
    public Student Student { get; set; }
    public Subject Subject { get; set; }
}
