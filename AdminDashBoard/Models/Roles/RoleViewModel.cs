using System.ComponentModel.DataAnnotations;

namespace AdminDashBoard.Models.Roles
{
    public class RoleViewModel
    {
        [Required(ErrorMessage ="Role name is required")]
        [StringLength(100,ErrorMessage ="Max Size is 100")]
        public string Name { get; set; } = default!;
    }
}
