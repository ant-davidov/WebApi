using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Entities.Secondary;
using Domain.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Infrastructure.Data.Seed
{



    public class SeedUsers
    {
        public static async void Initialize(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetRequiredService<DataContext>();
            var userManager = serviceProvider.GetRequiredService<UserManager<Account>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
            var role1 = new ApplicationRole { Name = RoleEnum.ADMIN.ToString() };
            var role2 = new ApplicationRole { Name = RoleEnum.CHIPPER.ToString() };
            var role3 = new ApplicationRole { Name = RoleEnum.USER.ToString() };
            await roleManager.CreateAsync(role1);
            await roleManager.CreateAsync(role2);
            await roleManager.CreateAsync(role3);
            var a = roleManager.Roles.ToList();
            var user = new Account
            {
                FirstName = "adminFirstName",
                LastName = "adminLastName",
                Email = "admin@simbirsoft.com",
                UserName = "admin@simbirsoft.com"

            };

            var result = await userManager.CreateAsync(user, "qwerty123");
            await userManager.AddToRoleAsync(user, RoleEnum.ADMIN.ToString());
            var user2 = new Account
            {
                FirstName = "chipperFirstName",
                LastName = "chipperLastName",
                Email = "chipper@simbirsoft.com",
                UserName = "chipper@simbirsoft.com"

            };

            result = await userManager.CreateAsync(user2, "qwerty123");
            await userManager.AddToRoleAsync(user2, RoleEnum.CHIPPER.ToString());

            var user3 = new Account
            {
                FirstName = "userFirstName",
                LastName = "userLastName",
                Email = "user@simbirsoft.com",
                UserName = "user@simbirsoft.com"
            };

            result = await userManager.CreateAsync(user3, "qwerty123");
            await userManager.AddToRoleAsync(user3, RoleEnum.USER.ToString());
        }
    }
}
