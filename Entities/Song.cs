using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace _4kTiles_Backend.Entities
{
    [Table("song")]
    public partial class Song
    {
        public Song()
        {
            Accountsongs = new HashSet<Accountsong>();
            Songgenres = new HashSet<Songgenre>();
            Songreports = new HashSet<Songreport>();
            Songtags = new HashSet<Songtag>();
        }

        [Key]
        [Column("songid")]
        public int Songid { get; set; }
        [Column("songname")]
        [StringLength(255)]
        public string Songname { get; set; } = null!;
        [Column("author")]
        [StringLength(255)]
        public string Author { get; set; } = null!;
        [Column("bpm")]
        public int Bpm { get; set; }
        [Column("notes")]
        [StringLength(1000)]
        public string Notes { get; set; } = null!;
        [Column("releasedate", TypeName = "timestamp without time zone")]
        public DateTime Releasedate { get; set; }
        [Column("ispublic")]
        public bool Ispublic { get; set; }
        [Column("isdeleted")]
        public bool Isdeleted { get; set; }
        [Column("deletedreason")]
        [StringLength(255)]
        public string? Deletedreason { get; set; }

        [InverseProperty(nameof(Accountsong.Song))]
        public virtual ICollection<Accountsong> Accountsongs { get; set; }
        [InverseProperty(nameof(Songgenre.Song))]
        public virtual ICollection<Songgenre> Songgenres { get; set; }
        [InverseProperty(nameof(Songreport.Song))]
        public virtual ICollection<Songreport> Songreports { get; set; }
        [InverseProperty(nameof(Songtag.Song))]
        public virtual ICollection<Songtag> Songtags { get; set; }
    }
}
