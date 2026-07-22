using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.DTOs.IdentityDTOs
{
    public class UserDto 
    {
        public string Email { get; set; } = default!; 
        public string DisplayName { get; set; } = default!;
        public string Token { get; set; } = default!;

    }
}
