using AutoMapper;
using SchoolSystem.Web.Areas.Admin.ViewModels.Courses;
using SchoolSystem.Web.Models;
using SchoolSystem.Web.Models.EnumsClasses;

namespace SchoolSystem.Web.Areas.Admin.Helpers;

public class CourseMappingProfile : Profile
{
  private const string DefaultCoverImageUrl
    = "https://supershop0910.blob.core.windows.net/classes/course.jpg";

  public CourseMappingProfile()
  {
    CreateMap<Course, EditCourseViewModel>()
      .ForMember(dest => dest.CoverImageUrl,
        opt => opt.MapFrom(src =>
          src.CoverImageId != Guid.Empty
            ? $"https://supershop0910.blob.core.windows.net/{AzureContainerNames.classes}/{src.CoverImageId}"
            : DefaultCoverImageUrl)).ReverseMap();

    CreateMap<Course, CourseViewModel>()
      .ForMember(dest => dest.CoverImageUrl,
        opt => opt.MapFrom(src =>
          src.CoverImageId != Guid.Empty
            ? $"https://supershop0910.blob.core.windows.net/{AzureContainerNames.classes}/{src.CoverImageId}"
            : DefaultCoverImageUrl));
  }
}
