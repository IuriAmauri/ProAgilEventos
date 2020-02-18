using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace ProAgil.Domain.Identity
{
    public class Role : IdentityRole<long>
    {
        public List<UserRole> UserRoles { get; set; }
    }
}