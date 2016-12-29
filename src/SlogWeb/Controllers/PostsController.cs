using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SlogWeb.Data;
using Microsoft.AspNetCore.Identity;
using SlogWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using SlogWeb.FormObjects;
using AutoMapper;
using SlogWeb.ViewModels;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace SlogWeb.Controllers {
    [Authorize(Roles = "Administrators,Authors")]
    public class PostsController : Controller {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
        private readonly IHostingEnvironment _env;

        public PostsController(UserManager<ApplicationUser> userManager, ApplicationDbContext context, IMapper mapper, IHostingEnvironment env) {
            _userManager = userManager;
            _context = context;
            _mapper = mapper;
            _env = env;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index() {
            var posts = await _context.Posts.Include(p => p.Author).ToListAsync();
            List<PostPublicViewModel> ppvms = new List<PostPublicViewModel>();
            foreach (var post in posts) {
                ppvms.Add(_mapper.Map<Post, PostPublicViewModel>(post));
            }
            return View(ppvms);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id) {
            if (id == null) {
                return NotFound();
            }

            var post = await _context.Posts.Include(p => p.Author).Include(p => p.Comments).SingleOrDefaultAsync(p => p.Id == id);
            if (post == null) {
                return NotFound();
            }

            return View(_mapper.Map<Post, PostPublicViewModel>(post));
        }

        public IActionResult Create() {
            return View(new PostFormObject());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PostFormObject pfo) {
            if (pfo.AuthorId == null) {
                var author = await GetCurrentUserAsync();
                pfo.AuthorId = author.Id;
            }
            if (ModelState.IsValid) {
                var post = _mapper.Map<PostFormObject, Post>(pfo);
                _context.Add(post);
                await _context.SaveChangesAsync();
                if (pfo.FeaturedImageFile != null) {
                    await UploadFile(post.Id, pfo.FeaturedImageFile);
                }
                return RedirectToAction("Index");
            }
            return View(pfo);
        }

        public async Task<IActionResult> Edit(int? id) {
            if (id == null) {
                return NotFound();
            }

            var post = await _context.Posts.SingleOrDefaultAsync(m => m.Id == id);
            if (post == null) {
                return NotFound();
            }
            return View(_mapper.Map<Post, PostFormObject>(post));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, PostFormObject pfo) {
            if (pfo.AuthorId == null) {
                var author = await GetCurrentUserAsync();
                pfo.AuthorId = author.Id;
            }
            var post = _mapper.Map<PostFormObject, Post>(pfo);
            post.Id = id;

            if (ModelState.IsValid) {
                if (PostExists(post.Id)) {
                    if (pfo.FeaturedImageFile != null) {
                        await UploadFile(post.Id, pfo.FeaturedImageFile);
                    }
                    _context.Update(post);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Details", new { Id = id });
                } else {
                    return NotFound();
                }
            }
            return View(post);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id) {
            var post = await _context.Posts.SingleOrDefaultAsync(m => m.Id == id);
            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool PostExists(int id) {
            return _context.Posts.Any(e => e.Id == id);
        }

        private async Task<ApplicationUser> GetCurrentUserAsync() {
            return await _userManager.GetUserAsync(HttpContext.User);
        }

        private async Task UploadFile(int id, IFormFile file) {
            var fileName = $"{_env.ContentRootPath}/wwwroot/uploads/{id}/{file.FileName}";
            Directory.CreateDirectory(Path.GetDirectoryName(fileName));
            using (var stream = new FileStream(fileName, FileMode.Create)) {
                await file.CopyToAsync(stream);
            }
        }
    }
}
