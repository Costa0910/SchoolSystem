using SchoolSystem.Web.Data.Interfaces;
using SchoolSystem.Web.Models;

namespace SchoolSystem.Web.Data.Repository;

public class StaffRepository(AppDbContext context) : GenericRepository<Staff>(context), IStaffRepository;
