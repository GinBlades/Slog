﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SlogWeb.Models {
    public class Post {
        [Key, Required]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Slug { get; set; }
        public string Summary { get; set; }
        [Required]
        public string Body { get; set; }
        [Required]
        public DateTime PublishDate { get; set; }
        [Required]
        public PostStatus PostStatus { get; set; }
        [Required]
        public string AuthorId { get; set; }
        [ForeignKey("AuthorId")]
        public virtual ApplicationUser Author { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }

        [NotMapped]
        public string PublishString {
            get {
                return PublishDate != null ? PublishDate.ToString("yyyy-MM-dd") : null;
            }
        }
    }

    public enum PostStatus {
        Draft, Review, Publish, Archive
    }
}
