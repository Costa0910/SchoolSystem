using System.ComponentModel.DataAnnotations;
using SchoolSystem.Web.Data.Interfaces;

namespace SchoolSystem.Web.Models;

/// <summary>
/// Represents a grade that a student received in a subject
/// </summary>
public class Grade : IEntity
{
    public Guid Id { get; init; }
    public double Value { get; set; }
    [MaxLength(50)]
    public string Status { get; set; } // Pass, Fail
    public Student Student { get; set; }
    public Subject Subject { get; set; }
}
