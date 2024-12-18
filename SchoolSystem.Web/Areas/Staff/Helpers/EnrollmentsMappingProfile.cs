using AutoMapper;
using SchoolSystem.Web.Areas.Staff.ViewModels.Enrollments;
using SchoolSystem.Web.Models;
using SchoolSystem.Web.Models.EnumsClasses;

namespace SchoolSystem.Web.Areas.Staff.Helpers;

public class EnrollmentsMappingProfile : Profile
{
  private const string DefaultCoverImageUrl
    = "https://supershop0910.blob.core.windows.net/classes/course.jpg";

  public EnrollmentsMappingProfile()
  {
    var culture = System.Globalization.CultureInfo.CurrentCulture;
    CreateMap<Course, DetailsStudentsViewModel>()
      .ForMember(dest => dest.CoverImageUrl,
        opt => opt.MapFrom(src =>
          src.CoverImageId != Guid.Empty
            ? $"https://supershop0910.blob.core.windows.net/{AzureContainerNames.classes}/{src.CoverImageId}"
            : DefaultCoverImageUrl))
      .ForMember(dest => dest.StartDate,
        opt => opt.MapFrom(src => src.StartDate.ToString("d", culture)));

    CreateMap<Course, DetailsSubjectsViewModel>()
      .ForMember(dest => dest.CoverImageUrl,
        opt => opt.MapFrom(src =>
          src.CoverImageId != Guid.Empty
            ? $"https://supershop0910.blob.core.windows.net/{AzureContainerNames.classes}/{src.CoverImageId}"
            : DefaultCoverImageUrl))
      .ForMember(dest => dest.StartDate,
        opt => opt.MapFrom(src => src.StartDate.ToString("d", culture)));
  }
}
