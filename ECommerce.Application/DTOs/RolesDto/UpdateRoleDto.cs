using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.DTOs.RolesDto
{
    public class UpdateRoleDto
    {
        public string Id { get; set; } = default!;
        public string Name { get; set; } = default!;
        public bool IsSelected { get; set; }
    }
}
