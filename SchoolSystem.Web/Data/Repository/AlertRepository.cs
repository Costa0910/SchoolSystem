using Microsoft.EntityFrameworkCore;
using SchoolSystem.Web.Data.Interfaces;
using SchoolSystem.Web.Models;
using SchoolSystem.Web.Models.EnumsClasses;

namespace SchoolSystem.Web.Data.Repository;

public class AlertRepository(AppDbContext context)
  : GenericRepository<Alert>(context), IAlertRepository
{
  private readonly string AllRoles
    = $"{Roles.Admin},{Roles.Staff},{Roles.Student}";

  private readonly AppDbContext _context = context;

  public async Task<IEnumerable<Alert>> GetAlertsAsync(string role)
  {
    return await _context.Alerts
      .Where(a => a.SendTo == role || a.SendTo == AllRoles)
      .Include(a => a.CreatedBy)
      .OrderByDescending(a => a.DateCreated)
      .ToListAsync();
  }

  public async Task<IEnumerable<Alert>> GetAlertsCreatedByUser(User user)
  {
    return await _context.Alerts.Where(a => a.CreatedBy == user)
      .Include(a => a.CreatedBy)
      .OrderByDescending(a => a.DateCreated)
      .ToListAsync();
  }
}
