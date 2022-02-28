﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace _4kTiles_Backend.Entities
{
    [Table("role")]
    public partial class Role
    {
        public Role()
        {
            Accountroles = new HashSet<Accountrole>();
        }

        [Key]
        [Column("roleid")]
        public int Roleid { get; set; }
        [Column("rolename")]
        [StringLength(50)]
        public string Rolename { get; set; } = null!;

        [InverseProperty(nameof(Accountrole.Role))]
        public virtual ICollection<Accountrole> Accountroles { get; set; }
    }
}
