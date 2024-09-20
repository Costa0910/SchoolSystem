using AutoMapper;
using SchoolSystem.Web.Areas.Admin.ViewModels.Users;
using SchoolSystem.Web.Models;
using SchoolSystem.Web.Models.EnumsClasses;

namespace SchoolSystem.Web.Areas.Admin.Helpers;
public class UserMappingProfile : Profile
{
  private const string  DefaultProfilePhotoUrl = "http://localhost:5286/img/avatars/3.png";

    public UserMappingProfile()
    {
      CreateMap<UserViewModel, User>();
      CreateMap<User, UserViewModel>()
        .ForMember(
            dest => dest.ProfilePhotoUrl,
            opt => opt.MapFrom(
                src => src.ProfilePhotoId != Guid.Empty
                           ? $"https://supershop0910.blob.core.windows.net/{AzureContainerNames.profile}/{src.ProfilePhotoId}"
                           : DefaultProfilePhotoUrl));
      CreateMap<User, EditUserViewModel>()
        .ForMember(
          dest => dest.ProfilePhotoUrl,
          opt => opt.MapFrom(
            src => src.ProfilePhotoId != Guid.Empty
              ? $"https://supershop0910.blob.core.windows.net/{AzureContainerNames.profile}/{src.ProfilePhotoId}"
              : DefaultProfilePhotoUrl)).ReverseMap();
      CreateMap<CreateAdminViewModel, UserViewModel>();
      CreateMap<CreateStaffViewModel, UserViewModel>();
      CreateMap<CreateStudentViewModel, UserViewModel>();
      CreateMap<AdminUser, UserDetailsViewModel>()
        .ForMember(
          dest => dest.ProfilePhotoUrl,
          opt => opt.MapFrom(
            src => src.User.ProfilePhotoId!= Guid.Empty
              ? $"https://supershop0910.blob.core.windows.net/{AzureContainerNames.profile}/{src.User.ProfilePhotoId}"
              : DefaultProfilePhotoUrl));

      CreateMap<Staff, UserDetailsViewModel>()
        .ForMember(
          dest => dest.ProfilePhotoUrl,
          opt => opt.MapFrom(
            src => src.User.ProfilePhotoId != Guid.Empty
              ? $"https://supershop0910.blob.core.windows.net/{AzureContainerNames.profile}/{src.User.ProfilePhotoId}"
              : DefaultProfilePhotoUrl));

      CreateMap<Student, UserDetailsViewModel>()
        .ForMember(
          dest => dest.ProfilePhotoUrl,
          opt => opt.MapFrom(
            src => src.User.ProfilePhotoId != Guid.Empty
              ? $"https://supershop0910.blob.core.windows.net/{AzureContainerNames.profile}/{src.User.ProfilePhotoId}"
              : DefaultProfilePhotoUrl));
    }
}