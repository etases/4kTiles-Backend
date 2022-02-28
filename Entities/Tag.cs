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
            SongTags = new HashSet<SongTag>();
        }

        [Key]
        [Column("tagid")]
        public int TagId { get; set; }
        [Column("tagname")]
        [StringLength(50)]
        public string TagName { get; set; } = null!;
        [Column("ispublishertag")]
        public bool IsPublisherTag { get; set; }

        [InverseProperty(nameof(SongTag.Tag))]
        public virtual ICollection<SongTag> SongTags { get; set; }
    }
}
