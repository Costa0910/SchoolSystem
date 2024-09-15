using System.ComponentModel.DataAnnotations;
using SchoolSystem.Web.Data.Interfaces;

namespace SchoolSystem.Web.Models;

/// <summary>
/// Represents a student in the system.
/// </summary>
public class Student : IEntity
{
    public Guid Id { get; init; }
    public Guid PhotoId { get; set; }
    [MaxLength(50)]
    public string Status { get; set; } // (e.g., "Active", "Graduated","Dropped", "Excluded by Attendance")
    public double AttendancePercentage { get; set; }
    public bool IsExcludedForAttendance { get; set; }
    public User User { get; init; }
    public List<Course> Courses { get; set; }       
}
