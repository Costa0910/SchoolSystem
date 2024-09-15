// using AutoMapper;
// using Web.Areas.Admin.ViewModels;
// using Web.Models;
// using Web.Models.Enums;
//
// namespace Web.Areas.Admin.Helpers;
//
// public class AdminMappingProfile : Profile
// {
//     public AdminMappingProfile()
//     {
//         CreateMap<User, CoordinatorViewModel>()
//             .ForMember(
//                 dest => dest.FullName,
//                 opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"))
//             .ForMember(
//                 dest => dest.IsActive,
//                 opt => opt.MapFrom(src => src.AbsenceStatus == Status.Active));
//
//         CreateMap<ClassCreateViewModel, Room>();
//         CreateMap<ClassEditViewModel, Room>().ReverseMap();
//         CreateMap<Room, ClassViewModel>()
//             .ForMember(
//                 dest => dest.ClassImageUrl,
//                 opt => opt.MapFrom(
//                     src => src.ImageId != Guid.Empty
//                                ? $"https://supershop0910.blob.core.windows.net/{ContainerNames.classes.ToString()}/{src.ImageId}"
//                                : "/images/classes/default.png"));
//
//         CreateMap<Discipline, DisciplineEditViewModel>().ReverseMap();
//     }
// }
