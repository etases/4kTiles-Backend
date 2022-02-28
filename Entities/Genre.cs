﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace _4kTiles_Backend.Entities
{
    [Table("genre")]
    public partial class Genre
    {
        public Genre()
        {
            Songgenres = new HashSet<Songgenre>();
        }

        [Key]
        [Column("genreid")]
        public int Genreid { get; set; }
        [Column("genrename")]
        [StringLength(50)]
        public string Genrename { get; set; } = null!;

        [InverseProperty(nameof(Songgenre.Genre))]
        public virtual ICollection<Songgenre> Songgenres { get; set; }
    }
}
