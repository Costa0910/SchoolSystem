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
public class UserHelper(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, SignInManager<User> signInManager)
    : IUserHelper
{
    public async Task<User?> GetUserByEmailAsync(string email)
        => await userManager.FindByEmailAsync(email);

    public async Task<User?> GetUserByIdAsync(string id)
        => await userManager.FindByIdAsync(id);

    public async Task<User> AddUserAsync(User user, string password)
        => throw new NotImplementedException();

    public async Task<User?> AddUserAsync(User user, string password, string roleName)
    {
        var result = await userManager.CreateAsync(user, password);

        if (!result.Succeeded)
            return null;

        var roleResult = await userManager.AddToRoleAsync(user, roleName);

        return roleResult.Succeeded ? user : null;
    }

    public async Task AddUserToRoleAsync(User user, string roleName)
        => throw new NotImplementedException();

    public async Task<bool> IsUserInRoleAsync(User user, string roleName)
        => throw new NotImplementedException();

    public async Task<string> GenerateEmailConfirmationTokenAsync(User user)
        => await userManager.GenerateEmailConfirmationTokenAsync(user);

    public async Task<string> GenerateEmailConfirmationTokenAsync(string userId)
        => throw new NotImplementedException();

    public async Task<User> GetUserByTokenAsync(string token)
        => throw new NotImplementedException();

    public async Task<IdentityResult> ConfirmEmailAsync(User user, string token)
        => await userManager.ConfirmEmailAsync(user, token);

    public async Task<IdentityResult> ConfirmEmailAsync(string userId, string token)
        => throw new NotImplementedException();

    public async Task<string> GeneratePasswordResetTokenAsync(User user)
        => await userManager.GeneratePasswordResetTokenAsync(user);

    public async Task<IdentityResult> ResetPasswordAsync(User user, string token, string password)
        => await userManager.ResetPasswordAsync(user, token, password);

    public async Task<SignInResult> LoginAsync(string modelEmail, string modelPassword, bool modelRememberMe)
        => await signInManager.PasswordSignInAsync(modelEmail, modelPassword, modelRememberMe, false);

    public async Task LogoutAsync()
        => await signInManager.SignOutAsync();

    public async Task<IdentityResult> UpdateUserAsync(User user)
        => await userManager.UpdateAsync(user);

    public async Task<IdentityResult> ChangePasswordAsync(User user, string oldPassword, string newPassword)
        => await userManager.ChangePasswordAsync(user, oldPassword, newPassword);

    public async Task<bool> CheckPasswordAsync(User user, string password)
        => await userManager.CheckPasswordAsync(user, password);

    public async Task<string> GenerateTwoFactorTokenAsync(User user)
        => throw new NotImplementedException();

    public async Task<SignInResult> TwoFactorSignInAsync(string provider, string code, bool isPersistent, bool rememberClient)
        => throw new NotImplementedException();

    public async Task<IdentityResult> AddLoginAsync(User user, ExternalLoginInfo info)
        => throw new NotImplementedException();

    public async Task<ExternalLoginInfo> GetExternalLoginInfoAsync(string provider)
        => throw new NotImplementedException();

    public async Task<IdentityResult> RemoveLoginAsync(User user, string loginProvider, string providerKey)
        => throw new NotImplementedException();

    public async Task<User> GetUserByLoginAsync(string loginProvider, string providerKey)
        => throw new NotImplementedException();

    public Task<IEnumerable<User>> GetAllUsersAsync()
        => Task.FromResult<IEnumerable<User>>(userManager.Users.ToList());
    public async Task<IdentityResult> DeleteUserAsync(User user)
        => await userManager.DeleteAsync(user);

    public async Task<IdentityResult> AddToRoleAsync(User user, string roleName)
        => throw new NotImplementedException();

    public async Task<IdentityResult> RemoveFromRoleAsync(User user, string roleName)
        => throw new NotImplementedException();

    public async Task<IdentityResult> CreateRoleAsync(string roleName)
        => await roleManager.CreateAsync(new(roleName));

    public async Task<IdentityResult> DeleteRoleAsync(string roleName)
        => throw new NotImplementedException();

    public async Task<bool> RoleExistsAsync(string roleName)
        => throw new NotImplementedException();

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

    public async Task<IEnumerable<User>> GetAllUsersNotInRoleAsync(string roleName)
        => throw new NotImplementedException();

    public async Task<User> GetUserByUserNameAsync(string userName)
        => throw new NotImplementedException();

    // public async Task<UserDashboardPath> GetDashboardAsync(User user)
    // {
    //     const string defaultController = "Dashboard";
    //     const string defaultAction = "Index";
    //
    //     var userRoles = await GetRolesAsync(user);
    //     var enumerable = userRoles.ToList();
    //     var role = enumerable.FirstOrDefault();
    //
    //     return role switch
    //     {
    //         nameof(Roles.Admin) => new() {Action = defaultAction, Controller = defaultController, Area = nameof(Roles.Admin)},
    //         nameof(Roles.Staff) => new() {Action = defaultAction, Controller = defaultController, Area = nameof(Roles.Staff)},
    //         nameof(Roles.Teacher) => new() {Action = defaultAction, Controller = defaultController, Area = nameof(Roles.Teacher)},
    //         nameof(Roles.Student) => new() {Action = defaultAction, Controller = defaultController, Area = nameof(Roles.Student)},
    //         _ => new() {Action = defaultAction, Controller = defaultController, Area = string.Empty}
    //     };
    // }
}
