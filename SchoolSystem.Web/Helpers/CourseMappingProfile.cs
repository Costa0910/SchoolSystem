using AutoMapper;
using SchoolSystem.Web.Models;
using SchoolSystem.Web.Models.EnumsClasses;
using SchoolSystem.Web.ViewModels.Home;

namespace SchoolSystem.Web.Helpers;

public class CourseMappingProfile : Profile
{
  private const string DefaultCoverImageUrl = "https://localhost:7228/assets/images/defaultPublic.jpg";

  public CourseMappingProfile()
  {
    CreateMap<Course, CourseViewModel>()
      .ForMember(dest => dest.CoverImageUrl, opt => opt.MapFrom(src => src
        .CoverImageId == Guid.Empty
        ? DefaultCoverImageUrl
        : $"https://supershop0910.blob.core.windows.net/{AzureContainerNames.classes}/{src.CoverImageId}"));
  }
}
