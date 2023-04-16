using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Entities.Secondary;
using Domain.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Infrastructure.Data.Seed
{



    public class SeedUsers
    {
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly UserManager<Account> _userManager;
        public SeedUsers(UserManager<Account> userManager, RoleManager<ApplicationRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public  async void Initialize()
        {

           
            if (await _userManager.Users.AnyAsync()) return;
            var role1 = new ApplicationRole { Name = RoleEnum.ADMIN.ToString() };
            var role2 = new ApplicationRole { Name = RoleEnum.CHIPPER.ToString() };
            var role3 = new ApplicationRole { Name = RoleEnum.USER.ToString() };
            await _roleManager.CreateAsync(role1);
            await _roleManager.CreateAsync(role2);
            await _roleManager.CreateAsync(role3);
            var a = _roleManager.Roles.ToList();
            var user = new Account
            {
                FirstName = "adminFirstName",
                LastName = "adminLastName",
                Email = "admin@simbirsoft.com",
                UserName = "admin@simbirsoft.com"

            };

            var result = await _userManager.CreateAsync(user, "qwerty123");
            await _userManager.AddToRoleAsync(user, RoleEnum.ADMIN.ToString());
            var user2 = new Account
            {
                FirstName = "chipperFirstName",
                LastName = "chipperLastName",
                Email = "chipper@simbirsoft.com",
                UserName = "chipper@simbirsoft.com"

            };

            result = await _userManager.CreateAsync(user2, "qwerty123");
            await _userManager.AddToRoleAsync(user2, RoleEnum.CHIPPER.ToString());

            var user3 = new Account
            {
                FirstName = "userFirstName",
                LastName = "userLastName",
                Email = "user@simbirsoft.com",
                UserName = "user@simbirsoft.com"
            };

            result = await _userManager.CreateAsync(user3, "qwerty123");
            await _userManager.AddToRoleAsync(user3, RoleEnum.USER.ToString());
        }
    }
}
