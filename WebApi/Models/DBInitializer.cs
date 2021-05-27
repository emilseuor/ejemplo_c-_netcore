using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Data;

namespace WebApi.Models
{
    public class DBInitializer
    {
        public static async void Seed(IApplicationBuilder applicationBuilder)
        {
            UserManager<IdentityUser> _userManager;
            RoleManager<IdentityRole> _roleManager;

            var scope = applicationBuilder.ApplicationServices.CreateScope();
            _userManager = (UserManager<IdentityUser>)scope.ServiceProvider.GetService(typeof(UserManager<IdentityUser>));
            _roleManager = (RoleManager<IdentityRole>)scope.ServiceProvider.GetService(typeof(RoleManager<IdentityRole>));

             //= applicationBuilder.ApplicationServices.GetRequiredService<UserManager<IdentityUser>>();
             //= applicationBuilder.ApplicationServices.GetRequiredService<RoleManager<IdentityRole>>();

            if (_roleManager.Roles.Count() <= 0)
            {
                IdentityRole adminRole = new IdentityRole();
                adminRole.Name = "Admin";
                await _roleManager.CreateAsync(adminRole);
                
                IdentityRole clientRole = new IdentityRole();
                clientRole.Name = "Client";
                await _roleManager.CreateAsync(clientRole);

                IdentityUser adminUser = new IdentityUser();
                adminUser.Email = "admin@admin.com";
                adminUser.UserName = "admin@admin.com";
                await _userManager.CreateAsync(adminUser, "administrator123");

                adminUser = await _userManager.FindByEmailAsync("admin@admin.com");

                IdentityUser clientUser = new IdentityUser();
                clientUser.Email = "client@client.com";
                clientUser.UserName = "client@client.com";

                await _userManager.CreateAsync(clientUser, "password123");

                clientUser = await _userManager.FindByEmailAsync("client@client.com");

                await _userManager.AddToRoleAsync(adminUser, "Admin");
                await _userManager.AddToRoleAsync(clientUser, "Client");

            }
        }

    }
}
