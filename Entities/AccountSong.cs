using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace _4kTiles_Backend.Entities
{
    [Table("accountsong")]
    public partial class Accountsong
    {
        [Key]
        [Column("asid")]
        public int Asid { get; set; }
        [Column("accountid")]
        public int Accountid { get; set; }
        [Column("songid")]
        public int Songid { get; set; }
        [Column("bestscore")]
        public int Bestscore { get; set; }

        [ForeignKey(nameof(Accountid))]
        [InverseProperty("Accountsongs")]
        public virtual Account Account { get; set; } = null!;
        [ForeignKey(nameof(Songid))]
        [InverseProperty("Accountsongs")]
        public virtual Song Song { get; set; } = null!;
    }
}
