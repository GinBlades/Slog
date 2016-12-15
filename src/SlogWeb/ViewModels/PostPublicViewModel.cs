using SlogWeb.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SlogWeb.ViewModels {
    public class PostPublicViewModel {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public DateTime PublishDate { get; set; }
        public virtual ApplicationUser Author { get; set; }
        public virtual IEnumerable<Comment> Commments { get; set; }
        public Comment NewComment { get; set; }
    }
}
