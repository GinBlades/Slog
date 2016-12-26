using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using SlogWeb.Data;
using Microsoft.EntityFrameworkCore;
using SlogWeb.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using SlogWeb.ViewModels;
using SlogWeb.FormObjects.Account;
using Microsoft.AspNetCore.Identity;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace SlogWeb.Controllers {
    [Authorize(Roles = "Administrators")]
    public class UsersController : Controller {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public UsersController(ApplicationDbContext context, UserManager<ApplicationUser> userManager) {
            _context = context;
            _userManager = userManager;
        }
        // GET: /<controller>/
        public async Task<IActionResult> Index() {
            var users = await _context.Users.Include(u => u.Roles).ToListAsync();
            var roles = await _context.Roles.ToListAsync();
            var userList = new List<UserViewModel>();
            foreach (var user in users) {
                userList.Add(new UserViewModel(user, roles, containsAllRoles: true));
            }
            return View(userList);
        }

        public async Task<IActionResult> Details(string id) {
            if (string.IsNullOrWhiteSpace(id)) {
                return NotFound();
            }
            var user = await _context.Users.Include(u => u.Roles).SingleOrDefaultAsync(u => u.Id == id);
            if (user == null) {
                return NotFound();
            }
            var roles = await _context.Roles.Where(r => user.Roles.Select(ur => ur.RoleId).Contains(r.Id)).ToListAsync();

            return View(new UserViewModel(user, roles, containsAllRoles: false));
        }

        public async Task<IActionResult> Create() {
            var roles = await _context.Roles.ToListAsync();
            return View(new AdminCreateUserFormObject(null, roles.Select(r => r.Name).ToArray()));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AdminCreateUserFormObject acufo) {
            if (ModelState.IsValid) {
                var user = acufo.ToUser();
                var result = await _userManager.CreateAsync(user, acufo.Password);
                if (result.Succeeded) {
                    await _userManager.AddToRolesAsync(user, acufo.SelectedRoles);
                    return RedirectToAction("Details", new { Id = user.Id });
                }
            }
            var roles = await _context.Roles.ToListAsync();
            acufo.AvailableRoleNames = roles.Select(r => r.Name);
            return View(acufo);
        }

        public async Task<IActionResult> Edit(string id) {
            if (id == null) {
                return NotFound();
            }

            var user = await _context.Users.SingleOrDefaultAsync(u => u.Id == id);
            if (user == null) {
                return NotFound();
            }
            return View(new AdminEditUserFormObject(user));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, AdminEditUserFormObject acufo) {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Id == id);
            
            if (ModelState.IsValid) {
                acufo.UpdateUser(ref user);
                await _context.SaveChangesAsync();
                // Separate password changing to another action
                //if (rfo.Password != null && rfo.ConfirmPassword != null) {
                //    await _userManager.ChangePasswordAsync(user, rfo.Password, rfo.Password);
                //}
                return RedirectToAction("Details", new { Id = id });
            }
            return View(new AdminEditUserFormObject(user));
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(string id) {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Id == id);
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
