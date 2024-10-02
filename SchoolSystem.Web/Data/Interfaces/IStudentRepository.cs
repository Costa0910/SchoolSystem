using SchoolSystem.Web.Models;

namespace SchoolSystem.Web.Data.Interfaces;

public interface IStudentRepository : IGenericRepository<Student>
{
  Task<Student?> GetStudentByIdIncludeUserAsync(string id);
  Task<Student?> GetStudentByIdIncludeUserAsync(Guid id);
  Task<bool> DeleteStudentAsync(string id);
  Task<Student?> GetStudentByUserEmailAsync(string email);
  Task<List<Student>> GetStudentsIncludeUserAsync();
  Task<List<Student>> GetStudentsNotInCourseAsync(Course course);
}
