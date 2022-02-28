using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace _4kTiles_Backend.Entities
{
    [Table("accountrole")]
    public partial class AccountRole
    {
        [Key]
        [Column("arid")]
        public int ArId { get; set; }
        [Column("accountid")]
        public int AccountId { get; set; }
        [Column("roleid")]
        public int RoleId { get; set; }

        [ForeignKey(nameof(AccountId))]
        [InverseProperty("AccountRoles")]
        public virtual Account Account { get; set; } = null!;
        [ForeignKey(nameof(RoleId))]
        [InverseProperty("AccountRoles")]
        public virtual Role Role { get; set; } = null!;
    }
}
