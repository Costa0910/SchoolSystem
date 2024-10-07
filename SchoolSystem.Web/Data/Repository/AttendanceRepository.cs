using SchoolSystem.Web.Data.Interfaces;
using SchoolSystem.Web.Models;

namespace SchoolSystem.Web.Data.Repository;

public class AttendanceRepository(AppDbContext context) : GenericRepository<Attendance>(context), IAttendanceRepository;
