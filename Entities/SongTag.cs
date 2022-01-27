using System;
using System.Collections.Generic;

namespace _4kTiles_Backend.Entities
{
    public partial class SongTag
    {
        public int StId { get; set; }
        public int SongId { get; set; }
        public int TagId { get; set; }

        public virtual Song Song { get; set; } = null!;
        public virtual Tag Tag { get; set; } = null!;
    }
}
