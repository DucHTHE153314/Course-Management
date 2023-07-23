using CourseManagementWebClientWebClient.Data;
using CourseManagementWebClientWebClient.Models;
using Microsoft.AspNetCore.Identity;
using System;

namespace CourseManagementWebClientWebClient.DataHelper
{
    public static class ContextSeed
    {
        public static async Task SeedRolesAsync(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            await roleManager.CreateAsync(new IdentityRole(Role.SUPERADMIN.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Role.TEACHER.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Role.STUDENT.ToString()));
        }


        public static async Task SeedSuperAdminAsync(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            //Seed Default User
            var defaultUser = new AppUser
            {
                UserName = "superadmin",
                Name  = "superadmin",
                Email = "superadmin@gmail.com",
                Address = "Viet Nam",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true
            };
            if (userManager.Users.All(u => u.Id != defaultUser.Id))
            {
                var user = await userManager.FindByEmailAsync(defaultUser.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultUser, "123Pa$$word.");
                    await userManager.AddToRoleAsync(defaultUser, Role.SUPERADMIN.ToString());
                    await userManager.AddToRoleAsync(defaultUser, Role.TEACHER.ToString());
                    await userManager.AddToRoleAsync(defaultUser, Role.STUDENT.ToString());
                }

            }
        }
    }
}
