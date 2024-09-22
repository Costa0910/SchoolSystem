using AutoMapper;
using SchoolSystem.Web.Models;
using SchoolSystem.Web.Models.EnumsClasses;
using SchoolSystem.Web.ViewModels.Account;

namespace SchoolSystem.Web.Helpers;

public class AccountProfileMapping : Profile
{
    private const string  DefaultProfilePhotoUrl = "http://localhost:5286/img/avatars/3.png";

    public AccountProfileMapping()
    {
        CreateMap<AdminUser, AccountViewModel>()
          .ForMember(
            dest => dest.ProfilePhotoUrl,
            opt => opt.MapFrom(
              src => src.User.ProfilePhotoId != Guid.Empty
                ? $"https://supershop0910.blob.core.windows.net/{AzureContainerNames.profile}/{src.User.ProfilePhotoId}"
                : DefaultProfilePhotoUrl))
          .ForMember(
            dest => dest.Role,
            opt => opt.MapFrom(
              src => Roles.Admin));

        CreateMap<Staff, AccountViewModel>()
          .ForMember(
            dest => dest.ProfilePhotoUrl,
            opt => opt.MapFrom(
              src => src.User.ProfilePhotoId != Guid.Empty
                ? $"https://supershop0910.blob.core.windows.net/{AzureContainerNames.profile}/{src.User.ProfilePhotoId}"
                : DefaultProfilePhotoUrl))
          .ForMember(
            dest => dest.Role,
            opt => opt.MapFrom(
              src => Roles.Staff));

        CreateMap<Student, AccountViewModel>()
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
                             : DefaultProfilePhotoUrl))
          .ForMember(
            dest => dest.Role,
            opt => opt.MapFrom(
              src => Roles.Student));

        CreateMap<User, EditAccountViewModel>()
          .ForMember(
            dest => dest.ProfilePhotoUrl,
            opt => opt.MapFrom(
              src => src.ProfilePhotoId != Guid.Empty
                ? $"https://supershop0910.blob.core.windows.net/{AzureContainerNames.profile}/{src.ProfilePhotoId}"
                : DefaultProfilePhotoUrl));
    }

}
