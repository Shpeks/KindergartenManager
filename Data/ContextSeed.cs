using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading.Tasks;
using Diplom.Models;

namespace Diplom.Data
{
    public static class ContextSeed
    {
        public static async Task SeedRolesAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            //Seed Roles
            await roleManager.CreateAsync(new IdentityRole(Enums.Roles.Admin.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Enums.Roles.Guest.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Enums.Roles.Dev.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Enums.Roles.Medic.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Enums.Roles.Fabricator.ToString()));
            
        }
        public static async Task SeedSuperAdminAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            //Seed Default User
            var defaultUser = new ApplicationUser
            {
                UserName = "admin",
                Email = "admin@gmail.com",
                FirstName = "Danila",
                LastName = "Kobzev",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true
            };
            if (userManager.Users.All(u => u.Id != defaultUser.Id))
            {
                var user = await userManager.FindByEmailAsync(defaultUser.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultUser, "123Pa$$word");
                    await userManager.AddToRoleAsync(defaultUser, Enums.Roles.Dev.ToString());

                }
            }
        }
    }
}
