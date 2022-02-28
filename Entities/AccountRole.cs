using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace _4kTiles_Backend.Entities
{
    [Table("accountrole")]
    public partial class Accountrole
    {
        [Key]
        [Column("arid")]
        public int Arid { get; set; }
        [Column("accountid")]
        public int Accountid { get; set; }
        [Column("roleid")]
        public int Roleid { get; set; }

        [ForeignKey(nameof(Accountid))]
        [InverseProperty("Accountroles")]
        public virtual Account Account { get; set; } = null!;
        [ForeignKey(nameof(Roleid))]
        [InverseProperty("Accountroles")]
        public virtual Role Role { get; set; } = null!;
    }
}
