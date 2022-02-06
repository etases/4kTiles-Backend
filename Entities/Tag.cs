using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace _4kTiles_Backend.Entities
{
    [Table("Tag")]
    public partial class Tag
    {
        public Tag()
        {
            SongTags = new HashSet<SongTag>();
        }

        [Key]
        [Column("tagId")]
        public int TagId { get; set; }
        [Column("tagName")]
        [StringLength(50)]
        public string TagName { get; set; } = null!;
        [Column("isPublisherTag")]
        public bool IsPublisherTag { get; set; }

        [InverseProperty(nameof(SongTag.Tag))]
        public virtual ICollection<SongTag> SongTags { get; set; }
    }
}
