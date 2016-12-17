using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SlogWeb.Models {
    public class Comment {
        public int Id { get; set; }
        public string Name { get; set; }
        [Required]
        public string Body { get; set; }
        public bool Approved { get; set; }

        public int? ParentId { get; set; }
        public virtual Comment Parent { get; set; }
        [Required]
        public int PostId { get; set; }
        public virtual Post Post { get; set; }
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}
