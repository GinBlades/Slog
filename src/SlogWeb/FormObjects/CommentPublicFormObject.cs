using SlogWeb.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SlogWeb.FormObjects {
    public class CommentPublicFormObject {
        public string Name { get; set; }
        [Required]
        public string Body { get; set; }

        // Honey pot, kind of
        public string RequiredField { get; set; }

        public Comment ToComment() {
            return new Comment() {
                Name = Name,
                Body = Body,
                Approved = false
            };
        }
    }
}
