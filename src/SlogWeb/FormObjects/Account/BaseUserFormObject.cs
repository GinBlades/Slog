using SlogWeb.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SlogWeb.FormObjects.Account {
    public class BaseUserFormObject {
        public BaseUserFormObject() { }

        public BaseUserFormObject(ApplicationUser user) {
            Email = user.Email;
            UserName = user.UserName;
        }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Username")]
        public string UserName { get; set; }

        public ApplicationUser ToUser() {
            return new ApplicationUser() {
                Email = Email,
                UserName = UserName
            };
        }
    }
}
