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

  public async Task<Student?> GetStudentByIdIncludeUserAsync(Guid id)
  {
    return await _context.Students.Include(s => s.User).FirstOrDefaultAsync(s
      => s.Id == id);
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

  public async Task<List<Student>> GetStudentsIncludeUserAsync() =>
    await _context.Students.Include(s => s.User).ToListAsync();

  public async Task<List<Student>> GetStudentsNotInCourseAsync(Course course)
  {
    return await _context.Students
      .Include(s => s.User)
      .Where(s => s.Courses.All(c => c.Id != course.Id))
      .ToListAsync();
  }

  public async Task<Student?> GetStudentCoursesByEmailAsync(string email)
  {
    return await _context.Students
      .Include(s => s.Courses)
      .FirstOrDefaultAsync(s => s.User.Email == email);
  }

  public async Task<bool> CanDeleteStudentAsync(Student student, Guid courseId)
  {
    var course = await _context.Courses
      .Include(c => c.Students)
      .Include(c => c.Attendances).ThenInclude(attendance => attendance.Student)
      .Include(c => c.Grades).ThenInclude(grade => grade.Student)
      .FirstOrDefaultAsync(c => c.Id == courseId);

    if (course == null || !course.Students
          .Contains(student))
    {
      return false;
    }

    if (course.Attendances.Any(a => a.Student
                                     == student))
    {
      return false;
    }

    if (course.Grades.Any(g => g.Student == student))
    {
      return false;
    }

    return true;
  }

  public async Task<bool> CanDeleteStudentAsync(Student student)
  {
    var courses = await _context.Courses
      .Include(c => c.Students)
      .Include(c => c.Attendances).ThenInclude(attendance => attendance.Student)
      .Include(c => c.Grades).ThenInclude(grade => grade.Student)
      .ToListAsync();

    if (courses.All(c => !c.Students.Contains(student)))
    {
      return true;
    }

    if (courses.Any(c => c.Attendances.Any(a => a.Student == student)))
    {
      return false;
    }

    if (courses.Any(c => c.Grades.Any(g => g.Student == student)))
    {
      return false;
    }

    return true;
  }
}
