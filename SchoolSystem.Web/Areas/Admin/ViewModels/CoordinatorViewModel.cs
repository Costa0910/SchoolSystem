namespace Web.Areas.Admin.ViewModels;

public class CoordinatorViewModel
{
    public Guid Id { get; set; }
    public string ProfileImageId { get; set; }
    public string ProfileImage => $"/images/profile/{ProfileImageId}.jpg";
    public string FullName { get; set; }
    public string Email { get; set; }
    public bool IsActive { get; set; }
}