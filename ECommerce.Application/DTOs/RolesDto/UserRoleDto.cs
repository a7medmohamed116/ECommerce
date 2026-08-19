using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.DTOs.RolesDto
{
    public class UserRoleDto
    {
        public string UserId { get; set; } = default!;
        public string UserName { get; set; } = default!;
        public List<UpdateRoleDto> Roles { get; set; } = [];
    }
}
