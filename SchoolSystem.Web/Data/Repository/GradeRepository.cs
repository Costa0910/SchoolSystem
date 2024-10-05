using SchoolSystem.Web.Data.Interfaces;
using SchoolSystem.Web.Models;

namespace SchoolSystem.Web.Data.Repository;

public class GradeRepository(AppDbContext context) : GenericRepository<Grade>(context), IGradeRepository;
