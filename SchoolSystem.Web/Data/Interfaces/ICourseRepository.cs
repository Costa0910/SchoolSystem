using SchoolSystem.Web.Models;

namespace SchoolSystem.Web.Data.Interfaces;

public interface ICourseRepository : IGenericRepository<Course>
{
  Task<IEnumerable<Course>> GetCoursesWithSubjectsAndStudents();
  Task<Course?> GetCourseWithSubjects(Guid id);
  Task<Course?> GetCourseWithStudents(Guid id);
  Task<Course?> GetCourseWithStudentsDetails(Guid id);
  Task<Course?> GetCourseWithStudentsSubjectsAndGrades(Guid id);
  Task<Course?> GetCourseWithGradesAndStudents(Guid id);
}
