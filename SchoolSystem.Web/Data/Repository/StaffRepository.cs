using Microsoft.EntityFrameworkCore;
using SchoolSystem.Web.Data.Interfaces;
using SchoolSystem.Web.Models;

namespace SchoolSystem.Web.Data.Repository;

public class StaffRepository(AppDbContext context) : GenericRepository<Staff>(context), IStaffRepository
{
  private readonly AppDbContext _context = context;

  public async Task<Staff?> GetStaffByIdIncludeUserAsync(Guid id)
  {
    return await _context.Staffs.Include(s => s.User).FirstOrDefaultAsync(s => s
      .Id == id);
  }
}
