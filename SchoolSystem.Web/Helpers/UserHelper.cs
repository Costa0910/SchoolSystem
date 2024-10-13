using Microsoft.AspNetCore.Identity;
using SchoolSystem.Web.Helpers.Interfaces;
using SchoolSystem.Web.Models;

namespace SchoolSystem.Web.Helpers;

/// <summary>
/// Helper class for user management
/// </summary>
/// <param name="userManager">Class that manage user, creating, editing, etc.</param>
/// <param name="roleManager">Class that manage roles, creating, editing, etc.</param>
/// <param name="signInManager">Class that manage user login, logout</param>
public class UserHelper(
  UserManager<User> userManager,
  RoleManager<IdentityRole> roleManager,
  SignInManager<User> signInManager)
  : IUserHelper
{
  public async Task<User?> GetUserByEmailAsync(string email)
    => await userManager.FindByEmailAsync(email);

  public async Task<User?> GetUserByIdAsync(string id)
    => await userManager.FindByIdAsync(id);

  public async Task<User?> AddUserAsync(User user, string password,
    string roleName)
  {
    var result = await userManager.CreateAsync(user, password);

    if (!result.Succeeded)
      return null;

    var roleResult = await userManager.AddToRoleAsync(user, roleName);

    return roleResult.Succeeded ? user : null;
  }

  public async Task<string> GenerateEmailConfirmationTokenAsync(User user)
    => await userManager.GenerateEmailConfirmationTokenAsync(user);

  public async Task<IdentityResult> ConfirmEmailAsync(User user, string token)
    => await userManager.ConfirmEmailAsync(user, token);

  public async Task<string> GeneratePasswordResetTokenAsync(User user)
    => await userManager.GeneratePasswordResetTokenAsync(user);

  public async Task<IdentityResult> ResetPasswordAsync(User user, string token,
    string password)
    => await userManager.ResetPasswordAsync(user, token, password);

  public async Task<SignInResult> LoginAsync(string modelEmail,
    string modelPassword, bool modelRememberMe)
    => await signInManager.PasswordSignInAsync(modelEmail, modelPassword,
      modelRememberMe, false);

  public async Task LogoutAsync()
    => await signInManager.SignOutAsync();

  public async Task<IdentityResult> UpdateUserAsync(User user)
    => await userManager.UpdateAsync(user);

  public async Task<IdentityResult> ChangePasswordAsync(User user,
    string oldPassword, string newPassword)
    => await userManager.ChangePasswordAsync(user, oldPassword, newPassword);

  public async Task<bool> CheckPasswordAsync(User user, string password)
    => await userManager.CheckPasswordAsync(user, password);

  public Task<IEnumerable<User>> GetAllUsersAsync()
    => Task.FromResult<IEnumerable<User>>(userManager.Users.ToList());

  public async Task<IdentityResult> DeleteUserAsync(User user)
    => await userManager.DeleteAsync(user);

  public async Task<IdentityResult> CreateRoleAsync(string roleName) =>
    await roleManager.CreateAsync(new IdentityRole(roleName));

  public async Task<IEnumerable<string>> GetRolesAsync(User user)
    => await userManager.GetRolesAsync(user);

  public async Task<IEnumerable<User>> GetAllUsersInRoleAsync(string roleName)
  {
    var users = userManager.Users.ToList();
    var usersInRole = new List<User>();

    foreach (var user in users)
    {
      var isInRole = await userManager.IsInRoleAsync(user, roleName);

      if (isInRole)
        usersInRole.Add(user);
    }

    return usersInRole;
  }
}
