using System;
using System.Collections.Generic;

namespace _4kTiles_Backend.Entities
{
    public partial class Song
    {
        public Song()
        {
            AccountSongs = new HashSet<AccountSong>();
            SongReports = new HashSet<SongReport>();
            SongTags = new HashSet<SongTag>();
        }

        public int SongId { get; set; }
        public string SongName { get; set; } = null!;
        public string Author { get; set; } = null!;
        public int Bpm { get; set; }
        public string Notes { get; set; } = null!;
        public DateTime ReleaseDate { get; set; }
        public bool IsPublic { get; set; }
        public bool IsDeleted { get; set; }
        public string? DeletedReason { get; set; }

        public virtual ICollection<AccountSong> AccountSongs { get; set; }
        public virtual ICollection<SongReport> SongReports { get; set; }
        public virtual ICollection<SongTag> SongTags { get; set; }
    }
}
