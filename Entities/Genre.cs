using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace _4kTiles_Backend.Entities
{
    [Table("genre")]
    public partial class Genre
    {
        public Genre()
        {
            SongGenres = new HashSet<SongGenre>();
        }

        [Key]
        [Column("genreid")]
        public int GenreId { get; set; }
        [Column("genrename")]
        [StringLength(50)]
        public string GenreName { get; set; } = null!;

        [InverseProperty(nameof(SongGenre.Genre))]
        public virtual ICollection<SongGenre> SongGenres { get; set; }
    }
}
