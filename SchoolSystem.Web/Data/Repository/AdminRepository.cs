using SchoolSystem.Web.Data.Interfaces;
using SchoolSystem.Web.Models;

namespace SchoolSystem.Web.Data.Repository;

public class AdminRepository(AppDbContext context) : GenericRepository<Admin>(context), IAdminRepository;
