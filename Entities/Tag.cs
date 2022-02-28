using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace _4kTiles_Backend.Entities
{
    [Table("tag")]
    public partial class Tag
    {
        public Tag()
        {
            Songtags = new HashSet<Songtag>();
        }

        [Key]
        [Column("tagid")]
        public int Tagid { get; set; }
        [Column("tagname")]
        [StringLength(50)]
        public string Tagname { get; set; } = null!;
        [Column("ispublishertag")]
        public bool Ispublishertag { get; set; }

        [InverseProperty(nameof(Songtag.Tag))]
        public virtual ICollection<Songtag> Songtags { get; set; }
    }
}
