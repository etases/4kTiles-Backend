using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace _4kTiles_Backend.Entities
{
    [Table("accountsong")]
    public partial class AccountSong
    {
        [Key]
        [Column("asid")]
        public int AsId { get; set; }
        [Column("accountid")]
        public int AccountId { get; set; }
        [Column("songid")]
        public int SongId { get; set; }
        [Column("bestscore")]
        public int BestScore { get; set; }

        [ForeignKey(nameof(AccountId))]
        [InverseProperty("AccountSongs")]
        public virtual Account Account { get; set; } = null!;
        [ForeignKey(nameof(SongId))]
        [InverseProperty("AccountSongs")]
        public virtual Song Song { get; set; } = null!;
    }
}
