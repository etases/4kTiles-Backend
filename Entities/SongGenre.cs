using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace _4kTiles_Backend.Entities
{
    [Table("songgenre")]
    public partial class SongGenre
    {
        [Key]
        [Column("sgid")]
        public int SgId { get; set; }
        [Column("songid")]
        public int SongId { get; set; }
        [Column("genreid")]
        public int GenreId { get; set; }

        [ForeignKey(nameof(GenreId))]
        [InverseProperty("SongGenres")]
        public virtual Genre Genre { get; set; } = null!;
        [ForeignKey(nameof(SongId))]
        [InverseProperty("SongGenres")]
        public virtual Song Song { get; set; } = null!;
    }
}
