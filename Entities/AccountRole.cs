using System;
using System.Collections.Generic;

namespace _4kTiles_Backend.Entities
{
    public partial class AccountRole
    {
        public int ArId { get; set; }
        public int AccountId { get; set; }
        public int RoleId { get; set; }

        public virtual Account Account { get; set; } = null!;
        public virtual Role Role { get; set; } = null!;
    }
}
