using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace _4kTiles_Backend.Entities
{
    [Table("songtag")]
    public partial class Songtag
    {
        [Key]
        [Column("stid")]
        public int Stid { get; set; }
        [Column("songid")]
        public int Songid { get; set; }
        [Column("tagid")]
        public int Tagid { get; set; }

        [ForeignKey(nameof(Songid))]
        [InverseProperty("Songtags")]
        public virtual Song Song { get; set; } = null!;
        [ForeignKey(nameof(Tagid))]
        [InverseProperty("Songtags")]
        public virtual Tag Tag { get; set; } = null!;
    }
}
