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

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace SlogWeb.Controllers {
    [Authorize(Roles = "Administrators")]
    public class UsersController : Controller {
        private readonly ApplicationDbContext _context;

        public UsersController(ApplicationDbContext context) {
            _context = context;
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

        public IActionResult Create() {
            return View(new ApplicationUser());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ApplicationUser user) {
            if (ModelState.IsValid) {
                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", new { Id = user.Id });
            }
            return View(user);
        }

        public async Task<IActionResult> Edit(string id) {
            if (id == null) {
                return NotFound();
            }

            var user = await _context.Users.SingleOrDefaultAsync(u => u.Id == id);
            if (user == null) {
                return NotFound();
            }
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, ApplicationUser user) {
            if (ModelState.IsValid) {
                _context.Update(user);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", new { Id = id });
            }
            return View(user);
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
