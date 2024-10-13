using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolSystem.Web.Controllers.Api.DTOs;
using SchoolSystem.Web.Data.Interfaces;

namespace SchoolSystem.Web.Controllers.Api;

[AllowAnonymous, Route("api/[controller]"), ApiController]
public class CourseController(
  ICourseRepository courseRepository,
  IMapper mapper)
  : ControllerBase
{
  [HttpGet]
  public async Task<IActionResult> GetStudents(string name)
  {
    if (!ModelState.IsValid) return BadRequest("Course name is required");

    var course = await courseRepository.GetCourseByNameWithStudentsAsyn(name);
    if (course is null) return NotFound("Course not found");

    var students = mapper.Map<IEnumerable<Student>>(course.Students);
    return Ok(students);
  }
}
