using System;
using System.Collections.Generic;

namespace _4kTiles_Backend.Entities
{
    public partial class Account
    {
        public Account()
        {
            AccountRoles = new HashSet<AccountRole>();
            AccountSongs = new HashSet<AccountSong>();
            SongReports = new HashSet<SongReport>();
        }

        public int AccountId { get; set; }
        public string UserName { get; set; } = null!;
        public string HashedPassword { get; set; } = null!;
        public DateTime? Dob { get; set; }
        public string Email { get; set; } = null!;
        public bool IsDeleted { get; set; }
        public string? DeletedReason { get; set; }

        public virtual ICollection<AccountRole> AccountRoles { get; set; }
        public virtual ICollection<AccountSong> AccountSongs { get; set; }
        public virtual ICollection<SongReport> SongReports { get; set; }
    }
}
