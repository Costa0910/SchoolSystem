using SchoolSystem.Web.Models;

namespace SchoolSystem.Web.Data.Interfaces;

public interface IStaffRepository : IGenericRepository<Staff>
{
  Task<Staff?> GetStaffByIdIncludeUserAsync(Guid id);
}
