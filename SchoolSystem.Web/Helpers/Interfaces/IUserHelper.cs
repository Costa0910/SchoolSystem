using Microsoft.AspNetCore.Identity;
using SchoolSystem.Web.Models;

namespace SchoolSystem.Web.Helpers.Interfaces;

/// <summary>
///   Helper for user management
/// </summary>
public interface IUserHelper
{
  Task<User?> GetUserByEmailAsync(string email);
  Task<User?> GetUserByIdAsync(string id);
  Task<User?> AddUserAsync(User user, string password, string roleName);
  Task<string> GenerateEmailConfirmationTokenAsync(User user);
  Task<IdentityResult> ConfirmEmailAsync(User user, string token);
  Task<string> GeneratePasswordResetTokenAsync(User user);

  Task<IdentityResult> ResetPasswordAsync(User user, string token,
    string password);

  Task LogoutAsync();
  Task<IdentityResult> UpdateUserAsync(User user);

  Task<IdentityResult> ChangePasswordAsync(User user, string oldPassword,
    string newPassword);

  Task<bool> CheckPasswordAsync(User user, string password);
  Task<IEnumerable<User>> GetAllUsersAsync();
  Task<IdentityResult> DeleteUserAsync(User user);
  Task<IdentityResult> CreateRoleAsync(string roleName);
  Task<IEnumerable<string>> GetRolesAsync(User user);

  Task<SignInResult> LoginAsync(string modelEmail, string modelPassword,
    bool modelRememberMe);
}
