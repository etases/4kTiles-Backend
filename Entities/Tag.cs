using System;
using System.Collections.Generic;

namespace _4kTiles_Backend.Entities
{
    public partial class Tag
    {
        public Tag()
        {
            SongTags = new HashSet<SongTag>();
        }

        public int TagId { get; set; }
        public string TagName { get; set; } = null!;

        public virtual ICollection<SongTag> SongTags { get; set; }
    }
}
