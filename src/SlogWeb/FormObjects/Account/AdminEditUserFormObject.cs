using SlogWeb.Models;

namespace SlogWeb.FormObjects.Account {
    public class AdminEditUserFormObject : BaseUserFormObject {
        public AdminEditUserFormObject() : base() { }

        public AdminEditUserFormObject(ApplicationUser user) : base(user) { }

        public void UpdateUser(ref ApplicationUser user) {
            user.Email = Email;
            user.UserName = UserName;
        }
    }
}
