using Castle.Core.Configuration;
using Microsoft.AspNetCore.Hosting;
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
        private readonly IHostingEnvironment _env;

        public DbSeeder(
            ApplicationDbContext dbContext,
            RoleManager<IdentityRole> roleManager,
            UserManager<ApplicationUser> userManager,
            IOptions<AdminOptions> adminOptions,
            IHostingEnvironment env) {
            _dbContext = dbContext;
            _roleManager = roleManager;
            _userManager = userManager;
            _adminOptions = adminOptions.Value;
            _env = env;
        }

        public async Task SeedAsync() {
            _dbContext.Database.EnsureCreated();

            // Clear database when needed.
            // await ClearDbAsync();

            if (await _dbContext.Users.CountAsync() == 0) {
                await CreateUsersAsync();
            }
        }

        private async Task ClearDbAsync() {
            // Do not run this in production
            if (_env.EnvironmentName != "Development") {
                return;
            }
            _dbContext.Comments.RemoveRange(_dbContext.Comments.ToList());
            _dbContext.Posts.RemoveRange(_dbContext.Posts.ToList());
            _dbContext.UserRoles.RemoveRange(_dbContext.UserRoles.ToList());
            _dbContext.Users.RemoveRange(_dbContext.Users.ToList());
            _dbContext.Roles.RemoveRange(_dbContext.Roles.ToList());

            await _dbContext.SaveChangesAsync();
        }

        private async Task CreateUsersAsync() {
            var adminRole = "Administrators";
            var authorsRole = "Authors";
            await _roleManager.CreateAsync(new IdentityRole(adminRole));
            await _roleManager.CreateAsync(new IdentityRole("Moderators"));
            await _roleManager.CreateAsync(new IdentityRole("Members"));
            await _roleManager.CreateAsync(new IdentityRole(authorsRole));

            var adminUser = new ApplicationUser() {
                UserName = _adminOptions.UserName,
                Email = _adminOptions.Email
            };

            var createResult = await _userManager.CreateAsync(adminUser, _adminOptions.Password);

            // CreateAsync may fail silently unless this is checked.
            // http://stackoverflow.com/questions/33074990/why-would-createasyncuser-password-sometimes-fail-identity-v2-mvc5
            if (!createResult.Succeeded) {
                var exceptionText = createResult.Errors.Aggregate("User Creation Failed", (current, error) => {
                    return current + $" - {error.Code}: {error.Description}";
                });
            }

            await _userManager.AddToRoleAsync(adminUser, adminRole);
            await _userManager.AddToRoleAsync(adminUser, authorsRole);
        }
    }
}
