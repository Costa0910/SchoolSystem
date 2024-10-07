using AutoMapper;
using SchoolSystem.Web.Areas.Student.ViewModels.Grades;
using SchoolSystem.Web.Models;
using SchoolSystem.Web.Models.EnumsClasses;

namespace SchoolSystem.Web.Areas.Student.Helpers;

public class GradesMappingProfile : Profile
{
  private const string DefaultCoverImageUrl
    = "http://localhost:5286/img/avatars/course.jpg";

  public GradesMappingProfile()
  {
    CreateMap<Course, MyCoursesViewModel>()
      .ForMember(dest => dest.CoverImageUrl, opt => opt.MapFrom(src => src
        .CoverImageId != Guid.Empty
        ? $"https://supershop0910.blob.core.windows.net/{AzureContainerNames
          .classes}/{src.CoverImageId}"
        : DefaultCoverImageUrl));
  }
}
