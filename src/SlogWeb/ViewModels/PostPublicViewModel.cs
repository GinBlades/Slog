﻿using SlogWeb.FormObjects;
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
        public CommentPublicFormObject NewComment { get; set; }
        public string Summary {
            get {
                if (Body == null) {
                    return null;
                } else {
                    return Body.Substring(0, Math.Min(Body.Length, 25));
                }
            }
        }
    }
}
