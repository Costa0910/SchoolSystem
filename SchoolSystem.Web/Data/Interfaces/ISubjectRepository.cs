using SchoolSystem.Web.Models;

namespace SchoolSystem.Web.Data.Interfaces;

public interface ISubjectRepository: IGenericRepository<Subject>
{
  Task<IEnumerable<Subject>?> GetSubjectsNotInCourseAsync(Course course);
}
