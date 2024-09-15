using SchoolSystem.Web.Data.Interfaces;
using SchoolSystem.Web.Models;

namespace SchoolSystem.Web.Data.Repository;

public class AlertRepository(AppDbContext context) : GenericRepository<Alert>(context), IAlertRepository;
