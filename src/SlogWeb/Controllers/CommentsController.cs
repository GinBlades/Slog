﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SlogWeb.FormObjects;
using SlogWeb.Models;
using SlogWeb.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace SlogWeb.Controllers {
    [Authorize(Roles = "Administrators,Moderators")]
    public class CommentsController : Controller {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public CommentsController(ApplicationDbContext db, RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager) {
            _db = db;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index() {
            var comments = await _db.Comments.Include(c => c.User).Include(c => c.Post).ToListAsync();
            return View(comments);
        }

        public async Task<IActionResult> Details(int id) {
            var comment = await _db.Comments.Include(c => c.User).Include(c => c.Post).FirstOrDefaultAsync(c => c.Id == id);
            return View(comment);
        }

        [HttpPost]
        [AllowAnonymous, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string postSlug, CommentPublicFormObject cpfo) {
            var post = await _db.Posts.SingleOrDefaultAsync(p => p.Slug == postSlug);
            if (ModelState.IsValid && string.IsNullOrWhiteSpace(cpfo.RequiredField) && post != null) {
                var comment = await CreateWithUserAsync(cpfo);
                comment.PostId = post.Id;
                _db.Comments.Add(comment);
                await _db.SaveChangesAsync();
            }
            return RedirectToAction("Details", "Posts", new { Slug = post.Slug, Date = post.PublishString });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Approve(int id, bool approve = true) {
            var comment = await _db.Comments.SingleOrDefaultAsync(c => c.Id == id);
            comment.Approved = approve;
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id) {
            var comment = await _db.Comments.SingleOrDefaultAsync(c => c.Id == id);
            _db.Comments.Remove(comment);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private async Task<Comment> CreateWithUserAsync(CommentPublicFormObject cpfo) {
            var comment = cpfo.ToComment();
            var user = await GetCurrentUserAsync();
            if (user != null) {
                comment.Name = user.UserName;
                comment.UserId = user.Id;
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
