using System;
using System.Collections.Generic;

namespace _4kTiles_Backend.Entities
{
    public partial class AccountSong
    {
        public int AsId { get; set; }
        public int AccountId { get; set; }
        public int SongId { get; set; }
        public int BestScore { get; set; }

        public virtual Account Account { get; set; } = null!;
        public virtual Song Song { get; set; } = null!;
    }
}
