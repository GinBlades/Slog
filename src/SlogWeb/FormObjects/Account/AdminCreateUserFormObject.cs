using SlogWeb.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SlogWeb.FormObjects.Account {
    public class AdminCreateUserFormObject : UserWithRolesFormObject {
        public AdminCreateUserFormObject() : base() { }

        public AdminCreateUserFormObject(ApplicationUser user) : base(user) { }
        public AdminCreateUserFormObject(ApplicationUser user, IEnumerable<string> roleNames) : this(user) {
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
    }
}
