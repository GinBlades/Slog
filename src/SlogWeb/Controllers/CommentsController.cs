using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SlogWeb.FormObjects;
using SlogWeb.Models;
using SlogWeb.Data;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace SlogWeb.Controllers {
    public class CommentsController : Controller {
        private readonly ApplicationDbContext _db;

        public CommentsController(ApplicationDbContext db) {
            _db = db;
        }

        [HttpPost]
        public async Task<IActionResult> Create(int postId, CommentPublicFormObject cpfo) {
            if (cpfo.RequiredField == null) {
                var comment = new Comment() {
                    Name = cpfo.Name,
                    Body = cpfo.Body,
                    PostId = postId,
                    Approved = false
                };

                _db.Comments.Add(comment);
                await _db.SaveChangesAsync();
            }

            return RedirectToAction("Details", "Posts", new { Id = postId });
        }
    }
}
