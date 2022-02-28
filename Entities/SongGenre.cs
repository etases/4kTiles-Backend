using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace _4kTiles_Backend.Entities
{
    [Table("songgenre")]
    public partial class Songgenre
    {
        [Key]
        [Column("sgid")]
        public int Sgid { get; set; }
        [Column("songid")]
        public int Songid { get; set; }
        [Column("genreid")]
        public int Genreid { get; set; }

        [ForeignKey(nameof(Genreid))]
        [InverseProperty("Songgenres")]
        public virtual Genre Genre { get; set; } = null!;
        [ForeignKey(nameof(Songid))]
        [InverseProperty("Songgenres")]
        public virtual Song Song { get; set; } = null!;
    }
}
