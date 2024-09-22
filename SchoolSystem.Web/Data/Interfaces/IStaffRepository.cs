using SchoolSystem.Web.Models;

namespace SchoolSystem.Web.Data.Interfaces;

public interface IStaffRepository : IGenericRepository<Staff>
{
  Task<Staff?> GetStaffByIdIncludeUserAsync(string id);
  Task<bool> DeleteStaffAsync(string id);
  Task<Staff?> GetStaffByUserEmailAsync(string email);
}
