using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Common
{
    public class IdentityUserResult //to deal with identityservice to validation then pass to authentication service that deal with the same everything (userdto) that data will returned to user after login
     {
        public IdentityUserResult(string id, string displayName, string email, string userName)
        {
            Id = id;
            DisplayName = displayName;
            Email = email;
            UserName = userName;
        }

        public string Id { get; set; } = default!;
        public string DisplayName { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string UserName { get; set; } = default!;


    }
}
