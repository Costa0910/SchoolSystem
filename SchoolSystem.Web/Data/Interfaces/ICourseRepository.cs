using System.Collections;
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
  Task<Course?> GetCourseWithStudentsDetailsAndSubjects(Guid id);
  Task<Course?> GetCourseWithStudentsSubjectsAndAbsent(Guid id);
  Task<Course?> GetAbsentsAsync(Guid courseId, Student student);
  Task<Course?> GetCourseByNameWithStudentsAsyn(string name);
}
