// using AutoMapper;
// using Microsoft.AspNetCore.Authorization;
// using Microsoft.AspNetCore.Mvc;
// using Web.Areas.Admin.ViewModels;
// using Web.Models;
// using Web.Models.Enums;
// using Web.Repositories.Interfaces;
//
// namespace Web.Areas.Admin.Controllers;
//
// [Area(nameof(Roles.Admin)), Authorize(Roles = nameof(Roles.Admin))]
// public class DisciplinesController(IDisciplineRepository disciplineRepository, IMapper mapper) : Controller
// {
//     // GET
//     public async Task<IActionResult> Index()
//     {
//         var disciplines = await disciplineRepository.GetAllAsync();
//
//         return View(disciplines);
//     }
//
//     public IActionResult Create()
//         => View();
//
//     [HttpPost, ValidateAntiForgeryToken]
//     public async Task<IActionResult> Create(DisciplineCreateViewModel model)
//     {
//         if (!ModelState.IsValid)
//             return View(model);
//
//         var discipline = new Discipline
//         {
//             Name = model.Name,
//             Description = model.Description
//         };
//
//         try
//         {
//             await disciplineRepository.AddAsync(discipline);
//         }
//         catch (Exception e)
//         {
//             if (e.InnerException is not null && e.InnerException.Message.Contains("duplicate key"))
//
//                 //Todo: Add alert message to the view
//                 ModelState.AddModelError(string.Empty, "Discipline already exists");
//             else
//                 ModelState.AddModelError(string.Empty, "An error occurred while creating the discipline");
//
//             return View(model);
//         }
//
//         return RedirectToAction(nameof(Index));
//     }
//
//     public async Task<IActionResult> Edit(string id)
//     {
//         if (!ModelState.IsValid)
//             return NotFound();
//
//         var discipline = await disciplineRepository.GetByIdAsync(Guid.Parse(id));
//
//         if (discipline is null)
//             return NotFound();
//
//         var model = mapper.Map<DisciplineEditViewModel>(discipline);
//
//         return View(model);
//     }
//
//     [HttpPost, ValidateAntiForgeryToken]
//     public async Task<IActionResult> Edit(DisciplineEditViewModel model)
//     {
//         if (!ModelState.IsValid)
//             return View(model);
//
//         var discipline = await disciplineRepository.GetByIdAsync(model.Id);
//
//         if (discipline is null)
//             return NotFound();
//
//         discipline.Name = model.Name;
//         discipline.Description = model.Description;
//
//         try
//         {
//             await disciplineRepository.UpdateAsync(discipline);
//         }
//         catch (Exception e)
//         {
//             if (e.InnerException is not null && e.InnerException.Message.Contains("duplicate key"))
//
//                 //Todo: Add alert message to the view
//                 ModelState.AddModelError(string.Empty, "Discipline already exists");
//             else
//                 ModelState.AddModelError(string.Empty, "An error occurred while updating the discipline");
//
//             return View(model);
//         }
//
//         return RedirectToAction(nameof(Index));
//     }
//
//     public async Task<IActionResult> Details(string id)
//     {
//         if (!ModelState.IsValid)
//             return NotFound();
//
//         var discipline = await disciplineRepository.GetByIdAsync(Guid.Parse(id));
//
//         if (discipline is null)
//             return NotFound();
//
//         return View(discipline);
//     }
//
//     public async Task<IActionResult> Delete(string id)
//     {
//         if (!ModelState.IsValid)
//             return NotFound();
//
//         var discipline = await disciplineRepository.GetByIdAsync(Guid.Parse(id));
//
//         if (discipline is null)
//             return NotFound();
//
//         try
//         {
//             await disciplineRepository.DeleteAsync(discipline);
//         }
//         catch (Exception e)
//         {
//             //Tdod: check if the discipline is used in any other entity
//             return RedirectToAction(nameof(Index), new { message = "An error occurred while deleting the discipline" });
//         }
//
//         return RedirectToAction(nameof(Index), new { message = $"{discipline.Name} deleted successfully" });
//     }
// }
