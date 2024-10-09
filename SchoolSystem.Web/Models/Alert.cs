using System.ComponentModel.DataAnnotations;
using SchoolSystem.Web.Data.Interfaces;

namespace SchoolSystem.Web.Models;

/// <summary>
/// Represents an alert that can be sent to students, staff, or admins.
/// </summary>
public class Alert : IEntity
{
    public Guid Id { get; init; }
    [MaxLength(1000)] public string Message { get; set; }
    public DateTime DateCreated { get; init; }
    public User CreatedBy { get; init; }
    [MaxLength(50)]
    public string SendTo { get; init; } // Student, Staff, Admin (e.g., "AllStudents", "All Staff", "All Admins")
}
