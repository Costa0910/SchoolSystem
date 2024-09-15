using SchoolSystem.Web.Data.Interfaces;
using SchoolSystem.Web.Models;

namespace SchoolSystem.Web.Data.Repository;

public class StudentRepository(AppDbContext context) : GenericRepository<Student>(context), IStudentRepository;
