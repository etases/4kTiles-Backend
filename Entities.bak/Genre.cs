using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace _4kTiles_Backend.Entities
{
    [Table("Genre")]
    public partial class Genre
    {
        public Genre()
        {
            SongGenres = new HashSet<SongGenre>();
        }

        [Key]
        [Column("genreId")]
        public int GenreId { get; set; }
        [Column("genreName")]
        [StringLength(50)]
        public string GenreName { get; set; } = null!;

        [InverseProperty(nameof(SongGenre.Genre))]
        public virtual ICollection<SongGenre> SongGenres { get; set; }
    }
}
