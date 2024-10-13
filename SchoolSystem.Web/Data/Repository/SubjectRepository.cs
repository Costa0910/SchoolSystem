using Microsoft.EntityFrameworkCore;
using SchoolSystem.Web.Data.Interfaces;
using SchoolSystem.Web.Models;

namespace SchoolSystem.Web.Data.Repository;

public class SubjectRepository(AppDbContext context)
  : GenericRepository<Subject>(context), ISubjectRepository
{
  private readonly AppDbContext _context = context;

  public async Task<IEnumerable<Subject>?> GetSubjectsNotInCourseAsync(
    Course course)
  {
    var subjects = await GetAllAsync();
    var courseSubjects = course.Subjects;

    return subjects.Where(s => courseSubjects.All(cs => cs.Id != s.Id));
  }

  public async Task<bool> CanDeleteSubjectAsync(Subject subject, Guid courseId)
  {
    var course = await _context.Courses
      .Include(c => c.Subjects)
      .Include(c => c.Attendances).ThenInclude(a => a.Subject)
      .Include(c => c.Grades).ThenInclude(g => g.Subject)
      .FirstOrDefaultAsync(c => c.Id == courseId);

    if (course == null || course.Subjects.All(s => s.Id != subject.Id))
    {
      return true;
    }

    if (course.Attendances.Any(a => a.Subject == subject)
        || course.Grades.Any(g => g.Subject == subject))
    {
      return false;
    }

    return true;
  }

  public async Task<bool> CanDeleteSubjectAsync(Subject subject)
  {
    var courses = await _context.Courses
      .Include(c => c.Subjects)
      .Include(c => c.Attendances).ThenInclude(a => a.Subject)
      .Include(c => c.Grades).ThenInclude(g => g.Subject)
      .ToListAsync();

    if (courses.All(c => c.Subjects.All(s => s.Id != subject.Id)))
    {
      return true;
    }

    if (courses.Any(c => c.Attendances.Any(a => a.Subject == subject))
        || courses.Any(c => c.Grades.Any(g => g.Subject == subject)))
    {
      return false;
    }

    return true;
  }
}
