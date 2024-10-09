using SchoolSystem.Web.Models;

namespace SchoolSystem.Web.ViewModels.Home;

public class HomeViewModel
{
  public IEnumerable<AlertViewModel> InBoxAlerts { get; set; }
  public IEnumerable<AlertViewModel> AlertsByUser { get; set; }
}
