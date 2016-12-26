using SlogWeb.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SlogWeb.FormObjects.Account {
    public class AdminCreateUserFormObject : BaseUserFormObject {
        public AdminCreateUserFormObject() : base() { }

        public AdminCreateUserFormObject(ApplicationUser user) : base(user) { }
        public AdminCreateUserFormObject(ApplicationUser user, string[] roleNames) {
            AvailableRoleNames = roleNames;
        }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "The password and confirmation do not match.")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "Roles")]
        public IEnumerable<string> AvailableRoleNames { get; set; } = new List<string>();

        public IEnumerable<string> SelectedRoles { get; set; } = new List<string>();
    }
}
