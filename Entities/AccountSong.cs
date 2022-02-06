using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace _4kTiles_Backend.Entities
{
    [Table("AccountSong")]
    public partial class AccountSong
    {
        [Key]
        [Column("asId")]
        public int AsId { get; set; }
        [Column("accountId")]
        public int AccountId { get; set; }
        [Column("songId")]
        public int SongId { get; set; }
        [Column("bestScore")]
        public int BestScore { get; set; }

        [ForeignKey(nameof(AccountId))]
        [InverseProperty("AccountSongs")]
        public virtual Account Account { get; set; } = null!;
        [ForeignKey(nameof(SongId))]
        [InverseProperty("AccountSongs")]
        public virtual Song Song { get; set; } = null!;
    }
}
