using SchoolSystem.Web.Models;

namespace SchoolSystem.Web.Data.Interfaces;

public interface ICourseRepository : IGenericRepository<Course>
{
  Task<IEnumerable<Course>> GetCoursesWithSubjectsAndStudents();
  Task<Course?> GetCourseWithSubjects(Guid id);
}
