using Microsoft.EntityFrameworkCore;
using SchoolSystem.Web.Data.Interfaces;
using SchoolSystem.Web.Models;
using Syncfusion.EJ2.Linq;

namespace SchoolSystem.Web.Data.Repository;

public class AdminRepository(AppDbContext context) :
  GenericRepository<AdminUser>(context), IAdminRepository
{
  private readonly AppDbContext _context = context;


  public async  Task<AdminUser?> GetAdminUserByIdIncludeUserAsync(string id)
  {
    return await _context.Admins.Include(a => a.User).FirstOrDefaultAsync(a=> a.User.Id == id);
  }

  public async Task<bool> DeleteAdminUserAsync(string id)
  {
    var admin = await _context.Admins.FirstOrDefaultAsync(a => a.User.Id == id);

    if (admin == null)
    {
      return false;
    }

    await DeleteAsync(admin);

    return true;
  }

  public async Task<AdminUser?> GetAdminUserByUserEmailAsync(string email)
  {
    return await _context.Admins.Include(a => a.User).FirstOrDefaultAsync(a => a
      .User.Email == email);
  }
}
