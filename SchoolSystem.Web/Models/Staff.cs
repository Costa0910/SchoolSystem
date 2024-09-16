using System.ComponentModel.DataAnnotations;
using SchoolSystem.Web.Data.Interfaces;

namespace SchoolSystem.Web.Models;

/// <summary>
/// Represents a staff member of the school.
/// </summary>
public class Staff : IEntity
{
    public Guid Id { get; init; }
    [MaxLength(50)]
    public string Status { get; set; } // (e.g., "Active", "Retired", "Fired")
    public DateTime HireDate { get; init; }
    [MaxLength(50)]
    public string Position { get; set; } // (e.g., "Teacher", "Staff", "Principal")

    public User User { get; init; }
}
