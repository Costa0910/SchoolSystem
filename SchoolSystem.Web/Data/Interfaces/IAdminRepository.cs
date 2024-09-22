using SchoolSystem.Web.Models;

namespace SchoolSystem.Web.Data.Interfaces;

public interface IAdminRepository : IGenericRepository<AdminUser>
{
  Task<AdminUser?> GetAdminUserByIdIncludeUserAsync(string id);
  Task<bool> DeleteAdminUserAsync(string id);
  Task<AdminUser?> GetAdminUserByUserEmailAsync(string email);
}
