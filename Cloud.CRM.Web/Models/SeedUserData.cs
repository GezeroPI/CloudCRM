using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cloud.CRM.Web.Models
{
    public static class SeedUserData
    {
        private const string adminUser = "Admin";
        private const string adminPassword = "Secret123$";
        private const string adminEmail = "info@ndh-webstudio.com";

        public static async void EnsurePopulated(IApplicationBuilder app)
        {
            IServiceScopeFactory scopeFactory = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>();

            using (IServiceScope scope = scopeFactory.CreateScope())
            {
                UserManager<ApplicationUser> userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

                RoleManager<ApplicationRole> userRole = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
                ApplicationRole role = await userRole.FindByNameAsync(adminUser);
                if (role == null)
                {
                    role = new ApplicationRole("Admin");
                    await userRole.CreateAsync(role);
                }

                ApplicationRole clientRole = await userRole.FindByNameAsync("User");
                if (userRole == null)
                {
                    clientRole = new ApplicationRole("User");
                    await userRole.CreateAsync(clientRole);
                }

                ApplicationUser user = await userManager.FindByNameAsync(adminUser);
                if (user == null)
                {
                    user = new ApplicationUser(adminUser, adminEmail);
                    var newuser = await userManager.CreateAsync(user, adminPassword);
                    if (newuser.Succeeded)
                    {
                        //here we tie the new user to the role
                        await userManager.AddToRoleAsync(user, "Admin");

                    }
                }
            }

        }
    }
}
