using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace _4kTiles_Backend.Entities
{
    [Table("AccountRole")]
    public partial class AccountRole
    {
        [Key]
        [Column("arId")]
        public int ArId { get; set; }
        [Column("accountId")]
        public int AccountId { get; set; }
        [Column("roleId")]
        public int RoleId { get; set; }

        [ForeignKey(nameof(AccountId))]
        [InverseProperty("AccountRoles")]
        public virtual Account Account { get; set; } = null!;
        [ForeignKey(nameof(RoleId))]
        [InverseProperty("AccountRoles")]
        public virtual Role Role { get; set; } = null!;
    }
}
