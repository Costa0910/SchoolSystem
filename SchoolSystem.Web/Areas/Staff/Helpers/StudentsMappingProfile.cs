using AutoMapper;
using SchoolSystem.Web.Areas.Staff.ViewModels.Students;
using SchoolSystem.Web.Models;
using SchoolSystem.Web.Models.EnumsClasses;

namespace SchoolSystem.Web.Areas.Staff.Helpers;

public class StudentsMappingProfile : Profile
{
  private const string DefaultImageUrl
    = "https://supershop0910.blob.core.windows.net/profile/user.png";

  private const string photoUrl = "http://localhost:5286/img/avatars/2.png";

  public StudentsMappingProfile()
  {
    CreateMap<CreateStudentViewModel, User>();

    CreateMap<User, EditStudentViewModel>()
      .ForMember(dest => dest.ProfilePhotoUrl, opt => opt.MapFrom(src => src
        .ProfilePhotoId == Guid.Empty
        ? DefaultImageUrl
        : $"https://supershop0910.blob.core.windows.net/{AzureContainerNames
          .profile}/{src.ProfilePhotoId}")).ReverseMap();

    CreateMap<Models.Student, StudentDetailsViewModel>()
      .ForMember(
        dest => dest.ProfilePhotoUrl,
        opt => opt.MapFrom(
          src => src.User.ProfilePhotoId != Guid.Empty
            ? $"https://supershop0910.blob.core.windows.net/{AzureContainerNames.profile}/{src.User.ProfilePhotoId}"
            : DefaultImageUrl))
      .ForMember(
        dest => dest.PhotoUrl,
        opt => opt.MapFrom(
          src => src.PhotoId != Guid.Empty
            ? $"https://supershop0910.blob.core.windows.net/{AzureContainerNames.profile}/{src.PhotoId}"
            : photoUrl))
      .ForMember(dest => dest.Role, opt => opt.MapFrom(src => Roles.Student));

    CreateMap<Models.Student, StudentsViewModel>()
      .ForMember(dest => dest.ProfilePhotoUrl, opt => opt.MapFrom(src => src
        .User.ProfilePhotoId == Guid.Empty
        ? DefaultImageUrl
        : $"https://supershop0910.blob.core.windows.net/{AzureContainerNames.profile}/{src.User.ProfilePhotoId}"))
      .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.User.Id))
      .ForMember(dest => dest.FirstName,
        opt => opt.MapFrom(src => src.User.FirstName))
      .ForMember(dest => dest.LastName,
        opt => opt.MapFrom(src => src.User.LastName))
      .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.User.Email));
  }
}
