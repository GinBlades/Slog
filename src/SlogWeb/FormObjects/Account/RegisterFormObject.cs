﻿using SlogWeb.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SlogWeb.FormObjects.Account {
    public class RegisterFormObject {
        public RegisterFormObject() { }

        public RegisterFormObject(ApplicationUser user) {
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

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "The password and confirmation do not match.")]
        public string ConfirmPassword { get; set; }

        public ApplicationUser ToUser() {
            return new ApplicationUser() {
                Email = Email,
                UserName = UserName
            };
        }
    }
}
