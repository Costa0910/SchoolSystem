using AutoMapper;
using SchoolSystem.Web.Models;
using SchoolSystem.Web.Models.EnumsClasses;
using SchoolSystem.Web.ViewModels.Home;

namespace SchoolSystem.Web.Helpers;

public class AlertsMappingProfile : Profile
{
  private const string DefaultProfilePhotoUrl
    = "http://localhost:5286/img/avatars/3.png";

  public AlertsMappingProfile()
  {
    CreateMap<Alert, AlertViewModel>()
      .ForMember(a => a.FullName,
        a => a.MapFrom(a => $"{a.CreatedBy.FirstName} {a.CreatedBy.LastName}"))
      .ForMember(
        dest => dest.PhotoUrl,
        opt => opt.MapFrom(
          src => src.CreatedBy.ProfilePhotoId != Guid.Empty
            ? $"https://supershop0910.blob.core.windows.net/{AzureContainerNames.profile}/{src.CreatedBy.ProfilePhotoId}"
            : DefaultProfilePhotoUrl));
  }
}
