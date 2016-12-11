using Castle.Core.Configuration;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using SlogWeb.Models;
using SlogWeb.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SlogWeb.Data {
    public class DbSeeder {
        private readonly ApplicationDbContext _dbContext;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AdminOptions _adminOptions;

        public DbSeeder(ApplicationDbContext dbContext, RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager, IOptions<AdminOptions> adminOptions) {
            _dbContext = dbContext;
            _roleManager = roleManager;
            _userManager = userManager;
            _adminOptions = adminOptions.Value;
        }

        public async Task SeedAsync() {
            _dbContext.Database.EnsureCreated();

            await ClearDb();

            if (await _dbContext.Users.CountAsync() == 0) {
                await CreateUsersAsync();
            }
        }

        private async Task ClearDb() {
            foreach (var user in _dbContext.Users.ToList()) {
                _dbContext.Users.Remove(user);
            }

            foreach (var role in _dbContext.Roles.ToList()) {
                _dbContext.Roles.Remove(role);
            }

            await _dbContext.SaveChangesAsync();
        }

        private async Task CreateUsersAsync() {
            var adminRole = "Administrators";
            await _roleManager.CreateAsync(new IdentityRole(adminRole));
            await _roleManager.CreateAsync(new IdentityRole("Moderators"));
            await _roleManager.CreateAsync(new IdentityRole("Members"));

            var adminUser = new ApplicationUser() {
                UserName = _adminOptions.UserName,
                Email = _adminOptions.Email
            };

            await _userManager.CreateAsync(adminUser, _adminOptions.Password);
            await _userManager.AddToRoleAsync(adminUser, adminRole);
        }
    }
}
