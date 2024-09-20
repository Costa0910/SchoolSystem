using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SchoolSystem.Web.Models;

namespace SchoolSystem.Web.Data;

/// <summary>
/// The database context for the application
/// </summary>
public class AppDbContext(DbContextOptions<AppDbContext> options)
    : IdentityDbContext<User>(options)
{
    public DbSet<AdminUser> Admins { get; set; }
    public DbSet<Staff> Staffs { get; set; }
    public DbSet<Student> Students { get; set; }
    public DbSet<Course> Courses { get; set; }
    public DbSet<Subject> Subjects { get; set; }
    public DbSet<Attendance> Attendances { get; set; }
    public DbSet<Grade> Grades { get; set; }
    public DbSet<Alert> Alerts { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        // add constraints unique on name
        builder.Entity<Course>()
               .HasIndex(r => r.Name)
               .IsUnique();

        builder.Entity<Subject>()
               .HasIndex(d => d.Name)
               .IsUnique();

        base.OnModelCreating(builder);
    }
}
