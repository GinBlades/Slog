using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SlogWeb.Models {
    public class ApplicationUser : IdentityUser {
        public virtual IEnumerable<Post> Posts { get; set; }
        public virtual IEnumerable<Comment> Comments { get; set; }
    }
}
