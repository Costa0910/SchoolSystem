using Microsoft.EntityFrameworkCore;
using SchoolSystem.Web.Data.Interfaces;
using SchoolSystem.Web.Models;

namespace SchoolSystem.Web.Data.Repository;

public class StudentRepository(AppDbContext context) : GenericRepository<Student>(context), IStudentRepository
{
  private readonly AppDbContext _context = context;
  public async Task<Student?> GetStudentByIdIncludeUserAsync(Guid id)
  {
    return await _context.Students.Include(s => s.User).FirstOrDefaultAsync(s => s.Id == id);
  }
}
