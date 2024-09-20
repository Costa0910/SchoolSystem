using System.ComponentModel.DataAnnotations;
using SchoolSystem.Web.Data.Interfaces;

namespace SchoolSystem.Web.Models;

/// <summary>
/// Admins of the application.
/// </summary>
public class AdminUser : IEntity
{
    public Guid Id { get; init; }

    [MaxLength(50)]
    public string Status { get; set; } // (e.g., "Active", "Retired", "Fired")

    [MaxLength(50)]
    public string AdminType { get; set; } // (e.g., "Super", "Normal")

    public User User { get; init; }
}
