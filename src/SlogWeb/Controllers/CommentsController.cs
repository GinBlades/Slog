using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SlogWeb.FormObjects;
using SlogWeb.Models;
using SlogWeb.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace SlogWeb.Controllers {
    public class CommentsController : Controller {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public CommentsController(ApplicationDbContext db, RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager) {
            _db = db;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> Create(int postId, CommentPublicFormObject cpfo) {
            if (cpfo.RequiredField == null) {
                var comment = await CreateWithUserAsync(cpfo);
                comment.PostId = postId;
                _db.Comments.Add(comment);
                await _db.SaveChangesAsync();
            }

            return RedirectToAction("Details", "Posts", new { Id = postId });
        }

        private async Task<Comment> CreateWithUserAsync(CommentPublicFormObject cpfo) {
            var comment = new Comment() {
                Name = cpfo.Name,
                Body = cpfo.Body,
                Approved = false
            };
            var user = await GetCurrentUserAsync();
            if (user != null) {
                cpfo.Name = user.UserName;
                var roles = await _userManager.GetRolesAsync(user);
                var adminRoles = new string[] { "Administrators", "Moderators" };
                if (roles.Intersect(adminRoles).Count() > 0) {
                    comment.Approved = true;
                }
            }
            return comment;
        }

        private async Task<ApplicationUser> GetCurrentUserAsync() {
            return await _userManager.GetUserAsync(HttpContext.User);
        }
    }
}
