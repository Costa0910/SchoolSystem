using Microsoft.EntityFrameworkCore;
using SchoolSystem.Web.Data.Interfaces;
using SchoolSystem.Web.Models;

namespace SchoolSystem.Web.Data.Repository;

public class StaffRepository(AppDbContext context)
  : GenericRepository<Staff>(context), IStaffRepository
{
  private readonly AppDbContext _context = context;

  public async Task<Staff?> GetStaffByIdIncludeUserAsync(string id)
  {
    return await _context.Staffs.Include(s => s.User).FirstOrDefaultAsync(s
      => s.User.Id == id);
  }

  public async Task<bool> DeleteStaffAsync(string id)
  {
    var staff = await _context.Staffs.FirstOrDefaultAsync(s => s.User.Id == id);
    if (staff == null)
    {
      return false;
    }

    await DeleteAsync(staff);

    return true;
  }

  public async Task<Staff?> GetStaffByUserEmailAsync(string email)
  {
    return await _context.Staffs.Include(s => s.User).FirstOrDefaultAsync(s
      => s.User.Email == email);
  }
}
