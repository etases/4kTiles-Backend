using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace _4kTiles_Backend.Entities
{
    [Table("Role")]
    public partial class Role
    {
        public Role()
        {
            AccountRoles = new HashSet<AccountRole>();
        }

        [Key]
        [Column("roleId")]
        public int RoleId { get; set; }
        [Column("roleName")]
        [StringLength(50)]
        public string RoleName { get; set; } = null!;

        [InverseProperty(nameof(AccountRole.Role))]
        public virtual ICollection<AccountRole> AccountRoles { get; set; }
    }
}
