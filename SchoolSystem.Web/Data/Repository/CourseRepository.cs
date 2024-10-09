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

  public async Task<Course?> GetCourseWithStudentsSubjectsAndGrades(Guid id)
  {
    return await context.Courses
      .Include(c => c.Students)
      .Include(c => c.Subjects)
      .Include(s => s.Grades)
      .FirstOrDefaultAsync(c => c.Id == id);
  }

  public async Task<Course?> GetCourseWithGradesAndStudents(Guid id)
  {
    return await context.Courses
      .Include(c => c.Students)
      .Include(c => c.Grades)
      .ThenInclude(g => g.Student)
      .Include(c => c.Grades)
      .ThenInclude(g => g.Subject)
      .FirstOrDefaultAsync(c => c.Id == id);
  }

  public async Task<Course?> GetCourseWithStudentsDetailsAndSubjects(Guid id)
  {
    return await context.Courses
      .Include(c => c.Students)
      .ThenInclude(s => s.User)
      .Include(c => c.Subjects)
      .FirstOrDefaultAsync(c => c.Id == id);
  }

  public async Task<Course?> GetCourseWithStudentsSubjectsAndAbsent(Guid id)
  {
    return await context.Courses
      .Include(c => c.Students)
      .Include(c => c.Subjects)
      .Include(c => c.Attendances)
      .FirstOrDefaultAsync(c => c.Id == id);
  }

  public async Task<Course?> GetAbsentsAsync(Guid courseId, Student student)
  {
    // return await context.Courses
    //   .Include(c => c.Attendances)
    //   .ThenInclude(a => a.Student == student)
    //   .FirstOrDefaultAsync(c => c.Id == courseId);

    return await context.Courses
      .Include(c => c.Attendances.Where(a => a.Student == student))
      .FirstOrDefaultAsync(c => c.Id == courseId);
  }
}
