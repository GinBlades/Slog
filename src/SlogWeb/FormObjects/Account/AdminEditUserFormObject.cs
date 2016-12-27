using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using SlogWeb.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace SlogWeb.FormObjects.Account {
    public class AdminEditUserFormObject : UserWithRolesFormObject {
        public AdminEditUserFormObject() : base() { }

        public AdminEditUserFormObject(ApplicationUser user) : base(user) { }
        public AdminEditUserFormObject(ApplicationUser user, IEnumerable<IdentityRole> roles) : this(user) {
            AvailableRoleNames = roles.Select(r => r.Name);
            if (user.Roles != null && user.Roles.Count() > 0) {
                SelectedRoles = roles.Where(r => user.Roles.Select(ur => ur.RoleId).Contains(r.Id)).Select(r => r.Name);
            }
        }

        public void UpdateUser(ref ApplicationUser user) {
            user.Email = Email;
            user.UserName = UserName;
        }
    }
}
