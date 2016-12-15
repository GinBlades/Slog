using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SlogWeb.Models {
    public class Comment {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Body { get; set; }
        public bool Approved { get; set; }

        public int ParentId { get; set; }
        public Comment Parent { get; set; }
        public int PostId { get; set; }
        public Post Post { get; set; }
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}
