using Microsoft.EntityFrameworkCore;
using SchoolSystem.Web.Data.Interfaces;
using SchoolSystem.Web.Models;

namespace SchoolSystem.Web.Data.Repository;

public class StudentRepository(AppDbContext context)
  : GenericRepository<Student>(context), IStudentRepository
{
  private readonly AppDbContext _context = context;

  public async Task<Student?> GetStudentByIdIncludeUserAsync(string id)
  {
    return await _context.Students.Include(s => s.User).FirstOrDefaultAsync(s
      => s.User.Id == id);
  }

  public async Task<bool> DeleteStudentAsync(string id)
  {
    var student
      = await _context.Students.FirstOrDefaultAsync(s => s.User.Id == id);
    if (student == null)
    {
      return false;
    }

    await DeleteAsync(student);
    return true;
  }

  public async Task<Student?> GetStudentByUserEmailAsync(string email)
  {
    return await _context.Students.Include(s => s.User).FirstOrDefaultAsync(s
      => s.User.Email == email);
  }
}
