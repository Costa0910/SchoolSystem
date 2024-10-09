using SchoolSystem.Web.Models;

namespace SchoolSystem.Web.Data.Interfaces;

public interface IAlertRepository : IGenericRepository<Alert>
{
  Task<IEnumerable<Alert>> GetAlertsAsync(string role);
  Task<IEnumerable<Alert>> GetAlertsCreatedByUser(User user);
}
