using System.Data;
using Microsoft.EntityFrameworkCore;
using SchoolSystem.Web.Helpers.Interfaces;
using SchoolSystem.Web.Models;
using SchoolSystem.Web.Models.EnumsClasses;

namespace SchoolSystem.Web.Data;

public static class Seed
{
    public static async Task SeedAsync(AppDbContext context,
        IUserHelper userHelper)
    {
        await context.Database.MigrateAsync();

        if (!await context.Roles.AnyAsync())
        {
            await userHelper.CreateRoleAsync(Roles.Admin);
            await userHelper.CreateRoleAsync(Roles.Staff);
            await userHelper.CreateRoleAsync(Roles.Student);
        }

        if (!await context.Users.AnyAsync())
        {
            var user = new User
            {
                FirstName = "Armando",
                LastName = "Costa",
                Email = "Costa0910@cinel.pt",
                UserName = "Costa0910@cinel.pt",
                ProfilePhotoId = Guid.Empty
            };

            user = await userHelper.AddUserAsync(user, "Password0910#", Roles.Admin);

            if (user is not null)
            {
                var token
                    = await userHelper
                        .GenerateEmailConfirmationTokenAsync(user);
                await userHelper.ConfirmEmailAsync(user, token);

                var admin = new AdminUser
                {
                    User = user,
                    Id = Guid.NewGuid(),
                    AdminType = AdminType.Super,
                    Status = AdminAndStaffStatus.Active
                };

                context.Admins.Add(admin);
                await context.SaveChangesAsync();
            }
            else
            {
                throw new DataException(
                    "The Admin could not be created in the database.");
            }
        }
    }
}
