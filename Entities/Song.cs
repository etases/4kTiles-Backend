using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace _4kTiles_Backend.Entities
{
    [Table("song")]
    public partial class Song
    {
        public Song()
        {
            AccountSongs = new HashSet<AccountSong>();
            SongGenres = new HashSet<SongGenre>();
            SongReports = new HashSet<SongReport>();
        }

        [Key]
        [Column("songid")]
        public int SongId { get; set; }
        [Column("songname")]
        [StringLength(255)]
        public string SongName { get; set; } = null!;
        [Column("author")]
        [StringLength(255)]
        public string Author { get; set; } = null!;
        [Column("creatorid")]
        public int CreatorId { get; set; }
        [Column("bpm")]
        public int Bpm { get; set; }
        [Column("notes")]
        [StringLength(1000)]
        public string Notes { get; set; } = null!;
        [Column("releasedate", TypeName = "timestamp without time zone")]
        public DateTime ReleaseDate { get; set; }
        [Column("updateddate", TypeName = "timestamp without time zone")]
        public DateTime UpdatedDate { get; set; }
        [Column("ispublic")]
        public bool IsPublic { get; set; }
        [Column("isdeleted")]
        public bool IsDeleted { get; set; }
        [Column("deletedreason")]
        [StringLength(255)]
        public string? DeletedReason { get; set; }

        [InverseProperty(nameof(AccountSong.Song))]
        public virtual ICollection<AccountSong> AccountSongs { get; set; }
        [InverseProperty(nameof(SongGenre.Song))]
        public virtual ICollection<SongGenre> SongGenres { get; set; }
        [InverseProperty(nameof(SongReport.Song))]
        public virtual ICollection<SongReport> SongReports { get; set; }
        [ForeignKey(nameof(CreatorId))]
        [InverseProperty("Songs")]
        public virtual Account Creator { get; set; }
    }
}
