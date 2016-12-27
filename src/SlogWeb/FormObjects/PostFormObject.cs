using SlogWeb.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SlogWeb.FormObjects {
    public class PostFormObject {
        [Required]
        public string Title { get; set; }

        private string _slug;
        public string Slug {
            get {
                if (string.IsNullOrWhiteSpace(_slug) && !string.IsNullOrWhiteSpace(Title)) {
                    // TODO: Make this better
                    _slug = Title.ToLower().Replace(" ", "-");
                }
                return _slug;
            }
            set { _slug = value; }
        }

        public string Summary { get; set; }

        [Required]
        public string Body { get; set; }
        public DateTime PublishDate { get; set; } = DateTime.Now;
        [Required]
        public PostStatus PostStatus { get; set; }
        public string AuthorId { get; set; }
    }
}
