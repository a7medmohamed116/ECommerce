using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.DTOs.IdentityDTOs
{
    public class RegisterDto
    {
        [Required(ErrorMessage ="Email Is Required"),EmailAddress]
        public string Email { get; set; } = default!;
        [Required]
        public string DisplayName { get; set; } = default!;

        [Required(ErrorMessage = "Password Is Required")]
        public string Password { get; set; } = default!;
        public string? PhoneNumber { get; set; }
        public string UserName { get; set; }
    }
}
