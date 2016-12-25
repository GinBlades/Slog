using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using SlogWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SlogWeb.ViewModels {
    public class UserViewModel {
        public UserViewModel() { }
        public UserViewModel(ApplicationUser user) {
            Id = user.Id;
            UserName = user.UserName;
            Email = user.Email;
        }

        // Using this(user) calls constructor matching that signature
        // 'containsAllRoles' flag to decide if passed in roles are specific to the user or
        // all roles for the application. Defaults to true, all application roles.
        public UserViewModel(ApplicationUser user, IEnumerable<IdentityRole> roles, bool containsAllRoles = true) : this(user) {
            if (containsAllRoles) {
                var userRoleIds = user.Roles.Select(r => r.RoleId);
                var userRoles = roles.Where(r => userRoleIds.Contains(r.Id));
                RoleNames = userRoles.Select(r => r.Name);
            } else {
                RoleNames = roles.Select(r => r.Name);
            }
        }

        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public IEnumerable<string> RoleNames { get; set; } = new List<string>();
    }
}
