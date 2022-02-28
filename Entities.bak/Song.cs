using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace _4kTiles_Backend.Entities
{
    [Table("Song")]
    public partial class Song
    {
        public Song()
        {
            AccountSongs = new HashSet<AccountSong>();
            SongGenres = new HashSet<SongGenre>();
            SongReports = new HashSet<SongReport>();
            SongTags = new HashSet<SongTag>();
        }

        [Key]
        [Column("songId")]
        public int SongId { get; set; }
        [Column("songName")]
        [StringLength(255)]
        public string SongName { get; set; } = null!;
        [Column("author")]
        [StringLength(255)]
        public string Author { get; set; } = null!;
        [Column("bpm")]
        public int Bpm { get; set; }
        [Column("notes")]
        [StringLength(1000)]
        public string Notes { get; set; } = null!;
        [Column("releaseDate", TypeName = "datetime")]
        public DateTime ReleaseDate { get; set; }
        [Column("isPublic")]
        public bool IsPublic { get; set; }
        [Column("isDeleted")]
        public bool IsDeleted { get; set; }
        [Column("deletedReason")]
        [StringLength(255)]
        public string? DeletedReason { get; set; }

        [InverseProperty(nameof(AccountSong.Song))]
        public virtual ICollection<AccountSong> AccountSongs { get; set; }
        [InverseProperty(nameof(SongGenre.Song))]
        public virtual ICollection<SongGenre> SongGenres { get; set; }
        [InverseProperty(nameof(SongReport.Song))]
        public virtual ICollection<SongReport> SongReports { get; set; }
        [InverseProperty(nameof(SongTag.Song))]
        public virtual ICollection<SongTag> SongTags { get; set; }
    }
}
