using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using SlogWeb.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SlogWeb.FormObjects.Account {
    public abstract class UserWithRolesFormObject : BaseUserFormObject {
        public UserWithRolesFormObject() : base() { }
        public UserWithRolesFormObject(ApplicationUser user) : base(user) { }

        [Display(Name = "Roles")]
        public IEnumerable<string> AvailableRoleNames { get; set; } = new List<string>();

        public IEnumerable<string> SelectedRoles { get; set; } = new List<string>();
    }
}
