using Microsoft.EntityFrameworkCore;
using SchoolSystem.Web.Data.Interfaces;
using SchoolSystem.Web.Models;

namespace SchoolSystem.Web.Data.Repository;

public class AdminRepository(AppDbContext context) :
  GenericRepository<AdminUser>(context), IAdminRepository
{
  private readonly AppDbContext _context = context;


  public async  Task<AdminUser?> GetAdminUserByIdIncludeUserAsync(Guid id)
  {
    return await _context.Admins.Include(a => a.User).FirstOrDefaultAsync(a => a
      .Id == id);
  }
}
