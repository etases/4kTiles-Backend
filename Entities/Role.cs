using System;
using System.Collections.Generic;

namespace _4kTiles_Backend.Entities
{
    public partial class Role
    {
        public Role()
        {
            AccountRoles = new HashSet<AccountRole>();
        }

        public int RoleId { get; set; }
        public string RoleName { get; set; } = null!;

        public virtual ICollection<AccountRole> AccountRoles { get; set; }
    }
}
