using SchoolSystem.Web.Data.Interfaces;
using SchoolSystem.Web.Models;

namespace SchoolSystem.Web.Data.Repository;

public class SubjectRepository(AppDbContext context) : GenericRepository<Subject>(context), ISubjectRepository;
