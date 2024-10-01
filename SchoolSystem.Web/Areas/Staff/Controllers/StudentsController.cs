using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolSystem.Web.Areas.Staff.ViewModels.Students;
using SchoolSystem.Web.Controllers;
using SchoolSystem.Web.Data.Interfaces;
using SchoolSystem.Web.Helpers.Interfaces;
using SchoolSystem.Web.Models.EnumsClasses;

namespace SchoolSystem.Web.Areas.Staff.Controllers;

[Area(Roles.Staff), Authorize(Roles = Roles.Staff)]
public class StudentsController(
  IUserHelper userHelper,
  IStudentRepository
    studentRepository,
  IMapper mapper) : BaseController
  (userHelper)
{
  public async Task<IActionResult> Index()
  {
    var students = await studentRepository.GetStudentsIncludeUserAsync();

    var studentsViewModel = students.Select(mapper.Map<StudentsViewModel>);

    return View(studentsViewModel);
  }

  public IActionResult Create()
  {
    throw new NotImplementedException();
  }
}
