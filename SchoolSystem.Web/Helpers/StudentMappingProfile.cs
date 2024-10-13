using AutoMapper;
using SchoolSystem.Web.Models.EnumsClasses;

namespace SchoolSystem.Web.Helpers;

public class StudentMappingProfile : Profile
{
  private const string DefaultProfilePhotoUrl
    = "https://supershop0910.blob.core.windows.net/profile/user.png";

  public StudentMappingProfile()
  {
    CreateMap<Models.Student, Controllers.Api.DTOs.Student>()
      .ForMember(dest => dest.FullName,
        opt => opt.MapFrom(src => src.User.FirstName + " " + src.User.LastName))
      .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.User.Email))
      .ForMember(
        dest => dest.ProfilePhotoUrl,
        opt => opt.MapFrom(
          src => src.User.ProfilePhotoId != Guid.Empty
            ? $"https://supershop0910.blob.core.windows.net/{AzureContainerNames.profile}/{src.User.ProfilePhotoId}"
            : DefaultProfilePhotoUrl))
      .ForMember(
        dest => dest.PhotoUrl,
        opt => opt.MapFrom(
          src => src.PhotoId != Guid.Empty
            ? $"https://supershop0910.blob.core.windows.net/{AzureContainerNames.profile}/{src.PhotoId}"
            : DefaultProfilePhotoUrl));
  }
}
