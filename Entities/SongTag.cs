using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace _4kTiles_Backend.Entities
{
    [Table("songtag")]
    public partial class SongTag
    {
        [Key]
        [Column("stid")]
        public int StId { get; set; }
        [Column("songid")]
        public int SongId { get; set; }
        [Column("tagid")]
        public int TagId { get; set; }

        [ForeignKey(nameof(SongId))]
        [InverseProperty("SongTags")]
        public virtual Song Song { get; set; } = null!;
        [ForeignKey(nameof(TagId))]
        [InverseProperty("SongTags")]
        public virtual Tag Tag { get; set; } = null!;
    }
}
