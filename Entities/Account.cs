using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace _4kTiles_Backend.Entities
{
    [Table("account")]
    public partial class Account
    {
        public Account()
        {
            Accountroles = new HashSet<Accountrole>();
            Accountsongs = new HashSet<Accountsong>();
            Songreports = new HashSet<Songreport>();
        }

        [Key]
        [Column("accountid")]
        public int Accountid { get; set; }
        [Column("username")]
        [StringLength(50)]
        public string Username { get; set; } = null!;
        [Column("hashedpassword")]
        [StringLength(255)]
        public string Hashedpassword { get; set; } = null!;
        [Column("dob", TypeName = "timestamp without time zone")]
        public DateTime? Dob { get; set; }
        [Column("email")]
        [StringLength(255)]
        public string Email { get; set; } = null!;
        [Column("isdeleted")]
        public bool Isdeleted { get; set; }
        [Column("deletedreason")]
        [StringLength(255)]
        public string? Deletedreason { get; set; }

        [InverseProperty(nameof(Accountrole.Account))]
        public virtual ICollection<Accountrole> Accountroles { get; set; }
        [InverseProperty(nameof(Accountsong.Account))]
        public virtual ICollection<Accountsong> Accountsongs { get; set; }
        [InverseProperty(nameof(Songreport.Account))]
        public virtual ICollection<Songreport> Songreports { get; set; }
    }
}
