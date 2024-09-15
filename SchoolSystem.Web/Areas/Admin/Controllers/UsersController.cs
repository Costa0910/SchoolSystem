// using AutoMapper;
// using Microsoft.AspNetCore.Authorization;
// using Microsoft.AspNetCore.Mvc;
// using Web.Areas.Admin.ViewModels;
// using Web.Helpers.Interfaces;
// using Web.Models.Enums;
// using Web.ViewModels;
//
// namespace Web.Areas.Admin.Controllers;
//
// [Area("Admin"), Authorize(Roles = nameof(Roles.Admin))]
// public class UsersController(IUserHelper userHelper, IMapper mapper) : Controller
// {
//     public async Task<IActionResult> Index(string? message)
//     {
//         if (!string.IsNullOrEmpty(message))
//         {
//             ViewBag.Message = message;
//         }
//
//         var users = await userHelper.GetAllUsersAsync();
//
//         users = users.Where(u => u.Email != User?.Identity?.Name);
//
//         var usersViewModel = mapper.Map<IEnumerable<CoordinatorViewModel>>(users);
//
//         return View(usersViewModel);
//     }
//
//     public IActionResult Create()
//     {
//         var model = new RegisterViewModel
//         {
//             Role = nameof(Roles.Coordinator)
//         };
//         return View(model);
//     }
//
//     [HttpPost]
//     public async Task<IActionResult> Create(RegisterViewModel model)
//     {
//         if (!ModelState.IsValid)
//         {
//             return View(model);
//         }
//
//         return View();
//     }
// }
