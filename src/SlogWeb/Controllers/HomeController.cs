using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SlogWeb.Data;
using SlogWeb.Models;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SlogWeb.ViewModels;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace SlogWeb.Controllers {
    public class HomeController : Controller {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;

        public HomeController(UserManager<ApplicationUser> userManager, ApplicationDbContext context, IMapper mapper) {
            _userManager = userManager;
            _context = context;
            _mapper = mapper;
        }
        // GET: /<controller>/
        public async Task<IActionResult> Index() {
            var posts = await _context.Posts.Include(p => p.Author).OrderByDescending(p => p.PublishDate).ToListAsync();
            List<PostPublicViewModel> ppvms = new List<PostPublicViewModel>();
            foreach (var post in posts) {
                ppvms.Add(_mapper.Map<Post, PostPublicViewModel>(post));
            }
            return View(ppvms);
        }
    }
}
