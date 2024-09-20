using SchoolSystem.Web.Models;

namespace SchoolSystem.Web.Data.Interfaces;

public interface IAdminRepository : IGenericRepository<AdminUser>
{
  Task<AdminUser?> GetAdminUserByIdIncludeUserAsync(Guid id);
}
