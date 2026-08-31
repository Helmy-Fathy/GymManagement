using GymManagement.DAL.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.DAL.Data.DataSeeding
{
    public static class IdentityDataSeeding
    {
        public static async Task SeedIdentityDataAsync(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager, ILogger logger, CancellationToken ct = default)
        {
            try
            {
                var hasUsers = await userManager.Users.AnyAsync(ct);
                var hasRoles = await roleManager.Roles.AnyAsync(ct);
                if (hasRoles && hasUsers) return;

                var roles = new List<IdentityRole>()
                {
                    new IdentityRole("SuperAdmin"),
                    new IdentityRole("Admin")
                };

                foreach (var role in roles)
                {
                    if (!await roleManager.RoleExistsAsync(role.Name!))
                    {
                        var roleResult = await roleManager.CreateAsync(role);
                        if (!roleResult.Succeeded)
                        {
                            // Log Error 
                            logger.LogError($"Failed To Create Role : {role.Name} : {string.Join(" ; ", roleResult.Errors.Select(e => e.Description))}");
                        }
                    }
                }

                if (!hasUsers)
                {
                    var MainAdmin = new ApplicationUser()
                    {
                        FirstName = "Helmy",
                        LastName = "Fathy",
                        Email = "7elmyfathy@gmail.com",
                        UserName = "HelmyFathy",
                        PhoneNumber = "01140137819"
                    };
                    await userManager.CreateAsync(MainAdmin, "P@ssw0rd");
                    await userManager.AddToRoleAsync(MainAdmin, "SuperAdmin");

                    var Admin = new ApplicationUser()
                    {
                        FirstName = "Mohamed",
                        LastName = "Fathy",
                        Email = "mohamedfathy@gmail.com",
                        UserName = "MohamedFathy",
                        PhoneNumber = "01066448576"
                    };
                    await userManager.CreateAsync(Admin, "P@ssw0rd");
                    await userManager.AddToRoleAsync(Admin, "Admin");

                    logger.LogInformation("Identtity Data Seeded");
                }

                return;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Identity Seeding Failed");
                return;
            }

        }
    }
}
