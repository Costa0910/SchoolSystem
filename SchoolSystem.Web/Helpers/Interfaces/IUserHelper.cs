using Microsoft.AspNetCore.Identity;
using SchoolSystem.Web.Models;

namespace SchoolSystem.Web.Helpers.Interfaces;

public interface IUserHelper
{
    Task<User?> GetUserByEmailAsync(string email);
    Task<User?> GetUserByIdAsync(string id);
    Task<User> AddUserAsync(User user, string password);
    Task<User?> AddUserAsync(User user, string password, string roleName);
    Task AddUserToRoleAsync(User user, string roleName);
    Task<bool> IsUserInRoleAsync(User user, string roleName);
    Task<string> GenerateEmailConfirmationTokenAsync(User user);
    Task<string> GenerateEmailConfirmationTokenAsync(string userId);
    Task<User> GetUserByTokenAsync(string token);
    Task<IdentityResult> ConfirmEmailAsync(User user, string token);
    Task<IdentityResult> ConfirmEmailAsync(string userId, string token);
    Task<string> GeneratePasswordResetTokenAsync(User user);
    Task<IdentityResult> ResetPasswordAsync(User user, string token, string password);
    Task LogoutAsync();
    Task<IdentityResult> UpdateUserAsync(User user);
    Task<IdentityResult> ChangePasswordAsync(User user, string oldPassword, string newPassword);
    Task<bool> CheckPasswordAsync(User user, string password);
    Task<string> GenerateTwoFactorTokenAsync(User user);
    Task<SignInResult> TwoFactorSignInAsync(string provider, string code, bool isPersistent, bool rememberClient);
    Task<IdentityResult> AddLoginAsync(User user, ExternalLoginInfo info);
    Task<ExternalLoginInfo> GetExternalLoginInfoAsync(string provider);
    Task<IdentityResult> RemoveLoginAsync(User user, string loginProvider, string providerKey);
    Task<User> GetUserByLoginAsync(string loginProvider, string providerKey);
    Task<IEnumerable<User>> GetAllUsersAsync();
    Task<IdentityResult> DeleteUserAsync(User user);
    Task<IdentityResult> AddToRoleAsync(User user, string roleName);
    Task<IdentityResult> RemoveFromRoleAsync(User user, string roleName);
    Task<IdentityResult> CreateRoleAsync(string roleName);
    Task<IdentityResult> DeleteRoleAsync(string roleName);
    Task<bool> RoleExistsAsync(string roleName);
    Task<IEnumerable<string>> GetRolesAsync(User user);
    Task<IEnumerable<User>> GetAllUsersInRoleAsync(string roleName);
    Task<IEnumerable<User>> GetAllUsersNotInRoleAsync(string roleName);
    Task<User> GetUserByUserNameAsync(string userName);
    //Task<UserDashboardPath> GetDashboardAsync(User user);
    Task<SignInResult> LoginAsync(string modelEmail, string modelPassword, bool modelRememberMe);
}
