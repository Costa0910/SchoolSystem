using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace SchoolSystem.Web.Models;

/// <summary>
/// Base class for all users of the application: Students, Teachers, Admins, coordinators.
/// </summary>
public class User : IdentityUser
{
    [MaxLength(50)]
    public string FirstName { get; set; }

    [MaxLength(50)]
    public string LastName { get; set; }

    public Guid? ProfilePhotoId { get; set; }
}
