using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace _4kTiles_Backend.Entities
{
    [Table("SongGenre")]
    public partial class SongGenre
    {
        [Key]
        [Column("sgId")]
        public int SgId { get; set; }
        [Column("songId")]
        public int SongId { get; set; }
        [Column("genreId")]
        public int GenreId { get; set; }

        [ForeignKey(nameof(GenreId))]
        [InverseProperty("SongGenres")]
        public virtual Genre Genre { get; set; } = null!;
        [ForeignKey(nameof(SongId))]
        [InverseProperty("SongGenres")]
        public virtual Song Song { get; set; } = null!;
    }
}
