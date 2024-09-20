using SchoolSystem.Web.Models;

namespace SchoolSystem.Web.Data.Interfaces;

public interface IStudentRepository : IGenericRepository<Student>
{
  Task<Student?> GetStudentByIdIncludeUserAsync(Guid id);
}
