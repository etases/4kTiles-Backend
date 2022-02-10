using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace _4kTiles_Backend.Entities
{
    [Table("Account")]
    public partial class Account
    {
        public Account()
        {
            AccountRoles = new HashSet<AccountRole>();
            AccountSongs = new HashSet<AccountSong>();
            SongReports = new HashSet<SongReport>();
        }

        [Key]
        [Column("accountId")]
        public int AccountId { get; set; }
        [Column("userName")]
        [StringLength(50)]
        public string UserName { get; set; } = null!;
        [Column("hashedPassword")]
        [StringLength(255)]
        public string HashedPassword { get; set; } = null!;
        [Column("dob", TypeName = "datetime")]
        public DateTime? Dob { get; set; }
        [Column("email")]
        [StringLength(255)]
        public string Email { get; set; } = null!;
        [Column("isDeleted")]
        public bool IsDeleted { get; set; }
        [Column("deletedReason")]
        [StringLength(255)]
        public string? DeletedReason { get; set; }

        [InverseProperty(nameof(AccountRole.Account))]
        public virtual ICollection<AccountRole> AccountRoles { get; set; }
        [InverseProperty(nameof(AccountSong.Account))]
        public virtual ICollection<AccountSong> AccountSongs { get; set; }
        [InverseProperty(nameof(SongReport.Account))]
        public virtual ICollection<SongReport> SongReports { get; set; }
    }
}
