// using AutoMapper;
// using Microsoft.AspNetCore.Authorization;
// using Microsoft.AspNetCore.Mvc;
// using Microsoft.AspNetCore.Mvc.Rendering;
// using Web.Areas.Admin.ViewModels;
// using Web.Helpers.Interfaces;
// using Web.Models;
// using Web.Models.Enums;
// using Web.Repositories.Interfaces;
//
// namespace Web.Areas.Admin.Controllers;
//
// [Area(nameof(Roles.Admin)), Authorize(Roles = nameof(Roles.Admin))]
// public class ClassesController(IRoomRepository roomRepository, IBlobStorageHelper blobStorageHelper, IMapper mapper, IDisciplineRepository disciplineRepository)
//     : Controller
// {
//     // GET
//     public async Task<IActionResult> Index()
//     {
//         var rooms = await roomRepository.GetRoomsWithMembersAndDisciplines();
//
//         return View(rooms);
//     }
//
//     public async Task<IActionResult> Edit(string id)
//     {
//         if (!ModelState.IsValid)
//         {
//             return RedirectToAction(nameof(Index), new { message = "Invalid room id." });
//         }
//
//         var room = await roomRepository.GetByIdAsync(Guid.Parse(id));
//
//         if (room is null)
//         {
//             return RedirectToAction(nameof(Index), new { message = "Room not found." });
//         }
//
//         var model = mapper.Map<ClassEditViewModel>(room);
//
//         return View(model);
//     }
//
//     [HttpPost, ValidateAntiForgeryToken]
//     public async Task<IActionResult> Edit(ClassEditViewModel model)
//     {
//         if (!ModelState.IsValid)
//         {
//             return View(model);
//         }
//
//         if (model.StartDate >= model.EndDate || model.StartDate < DateOnly.FromDateTime(DateTime.Now))
//         {
//             ModelState.AddModelError(nameof(model.StartDate), "Start date must be in the future and before the end date.");
//
//             return View(model);
//         }
//
//         var room = await roomRepository.GetByIdAsync(Guid.Parse(model.Id));
//
//         if (room is null)
//         {
//             return RedirectToAction(nameof(Index), new { message = "Room not found." });
//         }
//
//         var classImageId = room.ImageId;
//
//         if (model.ClassImage is not null && model.ClassImage.Length > 0)
//         {
//             if (room.ImageId != Guid.Empty && room.ImageId.HasValue)
//             {
//                 await blobStorageHelper.DeleteFileAsync(room.ImageId.Value, ContainerNames.classes);
//             }
//
//             classImageId = await blobStorageHelper.UploadFileAsync(model.ClassImage, ContainerNames.classes);
//         }
//
//         var updatedRoom = mapper.Map(model, room);
//         updatedRoom.ImageId = classImageId;
//
//         try
//         {
//             await roomRepository.UpdateAsync(updatedRoom);
//         }
//         catch (Exception e)
//         {
//             ModelState.AddModelError(string.Empty, e.Message);
//
//             return View(model);
//         }
//
//         return RedirectToAction(nameof(Index), new { message = $"{updatedRoom.Name} updated successfully." });
//     }
//
//     public async Task<IActionResult> Details(string id)
//     {
//         if (!ModelState.IsValid)
//         {
//             return RedirectToAction(nameof(Index), new { message = "Invalid room id." });
//         }
//
//         var room = await roomRepository.GetRoomWithDisciplines(Guid.Parse(id));
//
//         if (room is null)
//         {
//             return RedirectToAction(nameof(Index), new { message = "Room not found." });
//         }
//
//         var model = mapper.Map<ClassViewModel>(room);
//
//         return View(model);
//     }
//
//     public async Task<IActionResult> Delete(string id)
//     {
//         if (!ModelState.IsValid)
//         {
//             return RedirectToAction(nameof(Index), new { message = "Invalid room id." });
//         }
//
//         var room = await roomRepository.GetByIdAsync(Guid.Parse(id));
//
//         if (room is null)
//         {
//             return RedirectToAction(nameof(Index), new { message = "Room not found." });
//         }
//
//         if (room.ImageId != Guid.Empty && room.ImageId.HasValue)
//         {
//             await blobStorageHelper.DeleteFileAsync(room.ImageId.Value, ContainerNames.classes);
//         }
//
//         //Todo: Check if the room has any disciplines or members before deleting it
//         try
//         {
//             await roomRepository.DeleteAsync(room);
//         }
//         catch (Exception e)
//         {
//             return RedirectToAction(nameof(Index), new { message = e.Message });
//         }
//
//         return RedirectToAction(nameof(Index), new { message = $"{room.Name} deleted successfully." });
//     }
//
//     [HttpGet]
//     public IActionResult Create()
//         => View();
//
//     [HttpPost, ValidateAntiForgeryToken]
//     public async Task<IActionResult> Create(ClassCreateViewModel model)
//     {
//         if (!ModelState.IsValid)
//         {
//             return View(model);
//         }
//
//         if (model.StartDate >= model.EndDate || model.StartDate < DateOnly.FromDateTime(DateTime.Now))
//         {
//             ModelState.AddModelError(nameof(model.StartDate), "Start date must be in the future and before the end date.");
//
//             return View(model);
//         }
//
//         var classImageId = Guid.Empty;
//
//         if (model.ClassImage is not null && model.ClassImage.Length > 0)
//         {
//             classImageId = await blobStorageHelper.UploadFileAsync(model.ClassImage, ContainerNames.classes);
//         }
//
//         var newRoom = mapper.Map<Room>(model);
//         newRoom.Id = Guid.NewGuid();
//         newRoom.Disciplines = [];
//         newRoom.Members = [];
//         newRoom.ImageId = classImageId;
//
//         //Todo: Add constraints to prevent rooms with the same name
//         try
//         {
//             await roomRepository.AddAsync(newRoom);
//         }
//         catch (Exception e)
//         {
//             ModelState.AddModelError(string.Empty, e.Message);
//
//             return View(model);
//         }
//
//         return RedirectToAction(nameof(Index), new { message = $"{newRoom.Name} created successfully." });
//     }
//
//     public async Task<IActionResult> AddDiscipline()
//     {
//         var disciplines = await disciplineRepository.GetAllAsync();
//         var classes = await roomRepository.GetAllAsync();
//
//         var model = new AddDisciplineViewModel
//         {
//             Disciplines = disciplines.Select(d => new SelectListItem(d.Name, d.Id.ToString())).ToList(),
//             Classes = classes.Select(c => new SelectListItem(c.Name, c.Id.ToString())).ToList()
//         };
//
//         return View(model);
//     }
//
//     [HttpPost, ValidateAntiForgeryToken]
//     public async Task<IActionResult> AddDiscipline(AddDisciplineViewModel model)
//     {
//         var disciplines = await disciplineRepository.GetAllAsync();
//         var classes = await roomRepository.GetAllAsync();
//
//         if (!ModelState.IsValid)
//         {
//             model.Disciplines = disciplines.Select(d => new SelectListItem(d.Name, d.Id.ToString())).ToList();
//             model.Classes = classes.Select(c => new SelectListItem(c.Name, c.Id.ToString())).ToList();
//             return View(model);
//         }
//
//         var room = await roomRepository.GetRoomWithDisciplines(Guid.Parse(model.ClassId));
//
//         if (room is null)
//         {
//             return RedirectToAction(nameof(Index), new { message = "Room not found." });
//         }
//
//         var discipline = await disciplineRepository.GetByIdAsync(Guid.Parse(model.DisciplineId));
//
//         if (discipline is null)
//         {
//             return RedirectToAction(nameof(Index), new { message = "Discipline not found." });
//         }
//
//         if (room.Disciplines.Exists(d => d.Id == discipline.Id))
//         {
//             //Todo: Add alert message
//             ModelState.AddModelError(string.Empty, $"{discipline.Name} already exists in {room.Name}.");
//
//             model.Disciplines = disciplines.Select(d => new SelectListItem(d.Name, d.Id.ToString())).ToList();
//             model.Classes = classes.Select(c => new SelectListItem(c.Name, c.Id.ToString())).ToList();
//             return View(model);
//         }
//
//         room.Disciplines.Add(discipline);
//
//         try
//         {
//             await roomRepository.UpdateAsync(room);
//         }
//         catch (Exception e)
//         {
//             return RedirectToAction(nameof(Index), new { message = $"Could not add {discipline.Name} to ${room.Name}" });
//         }
//
//         return RedirectToAction(nameof(Index), new { message = $"{discipline.Name} added to {room.Name} successfully." });
//     }
//
//     public async Task<IActionResult> DeleteDiscipline(string disciplineId, string classId)
//     {
//         if (!ModelState.IsValid)
//         {
//             NotFound();
//         }
//
//         var room = await roomRepository.GetRoomWithDisciplines(Guid.Parse(classId));
//
//         if (room is null)
//             return NotFound();
//
//         var discipline = room.Disciplines.Find(d => d.Id == Guid.Parse(disciplineId));
//         if (discipline is null)
//             return NotFound();
//
//         room.Disciplines.Remove(discipline);
//
//         try
//         {
//             await roomRepository.UpdateAsync(room);
//         }
//         catch (Exception)
//         {
//             //Todo: check if alerts and absences has values
//             return RedirectToAction(nameof(Details), new { id = classId, message = $"Can't delete {discipline.Name}, it is been used in this class." });
//         }
//
//         return RedirectToAction(nameof(Details), new { id = classId, message = $"{discipline.Name} deleted successfully" });
//     }
// }
