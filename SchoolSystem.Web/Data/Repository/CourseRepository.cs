using Microsoft.EntityFrameworkCore;
using SchoolSystem.Web.Data.Interfaces;
using SchoolSystem.Web.Models;

namespace SchoolSystem.Web.Data.Repository;

public class CourseRepository(AppDbContext context)
  : GenericRepository<Course>(context), ICourseRepository
{
  public async Task<IEnumerable<Course>> GetCoursesWithSubjectsAndStudents()
  {
    return await context.Courses
      .Include(c => c.Subjects)
      .Include(c => c.Students)
      .ToListAsync();
  }

  public async Task<Course?> GetCourseWithSubjects(Guid id)
  {
    return await context.Courses
      .Include(c => c.Subjects)
      .Include(c => c.CreatedBy)
      .FirstOrDefaultAsync(c => c.Id == id);
  }

  public async Task<Course?> GetCourseWithStudents(Guid id)
  {
    return await context.Courses
      .Include(c => c.Students)
      .FirstOrDefaultAsync(c => c.Id == id);
  }

  public async Task<Course?> GetCourseWithStudentsDetails(Guid id)
  {
    return await context.Courses
      .Include(c => c.Students)
      .ThenInclude(s => s.User)
      .FirstOrDefaultAsync(c => c.Id == id);
  }
}
