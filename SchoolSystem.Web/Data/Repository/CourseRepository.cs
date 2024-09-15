using SchoolSystem.Web.Data.Interfaces;
using SchoolSystem.Web.Models;

namespace SchoolSystem.Web.Data.Repository;

public class CourseRepository(AppDbContext context) : GenericRepository<Course>(context), ICourseRepository;
