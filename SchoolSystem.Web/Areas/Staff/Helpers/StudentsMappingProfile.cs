using AutoMapper;
using SchoolSystem.Web.Areas.Staff.ViewModels.Students;
using SchoolSystem.Web.Models.EnumsClasses;

namespace SchoolSystem.Web.Areas.Staff.Helpers;

public class StudentsMappingProfile : Profile
{
  private const string DefaultImageUrl
    = "http://localhost:5286/img/avatars/course.jpg";

  public StudentsMappingProfile()
  {
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
