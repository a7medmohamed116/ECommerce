using AdminDashBoard.Models.Roles;
using System.ComponentModel.DataAnnotations;

namespace AdminDashBoard.Models.Users
{
    public class UserRoleViewModel
    {
        [Display(Name = "User Id")]
        public string UserId { get; set; }
        public string UserName { get; set; }
        public List<UpdateRoleViewModel> Roles { get; set; } //all roles in system
    }
}
