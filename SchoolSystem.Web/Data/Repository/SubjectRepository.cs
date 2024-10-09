using SchoolSystem.Web.Data.Interfaces;
using SchoolSystem.Web.Models;

namespace SchoolSystem.Web.Data.Repository;

public class SubjectRepository(AppDbContext context)
  : GenericRepository<Subject>(context), ISubjectRepository
{
  public async Task<IEnumerable<Subject>?> GetSubjectsNotInCourseAsync(
    Course course)
  {
    var subjects = await GetAllAsync();
    var courseSubjects = course.Subjects;

    return subjects.Where(s => courseSubjects.All(cs => cs.Id != s.Id));
  }
}
