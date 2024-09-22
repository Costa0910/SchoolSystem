using System.ComponentModel.DataAnnotations;

namespace SchoolSystem.Web.Areas.Admin.ViewModels.Users;

public class ViewUserViewModel : UserViewModel {
  public string Id { get; set; }

  public string? ProfilePhotoUrl { get; init; }
}
