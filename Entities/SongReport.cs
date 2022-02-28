using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace _4kTiles_Backend.Entities
{
    [Table("songreport")]
    public partial class Songreport
    {
        [Key]
        [Column("reportid")]
        public int Reportid { get; set; }
        [Column("songid")]
        public int Songid { get; set; }
        [Column("accountid")]
        public int Accountid { get; set; }
        [Column("reporttitle")]
        [StringLength(255)]
        public string Reporttitle { get; set; } = null!;
        [Column("reportreason")]
        [StringLength(255)]
        public string Reportreason { get; set; } = null!;
        [Column("reportdate", TypeName = "timestamp without time zone")]
        public DateTime Reportdate { get; set; }
        [Column("reportstatus")]
        public bool? Reportstatus { get; set; }

        [ForeignKey(nameof(Accountid))]
        [InverseProperty("Songreports")]
        public virtual Account Account { get; set; } = null!;
        [ForeignKey(nameof(Songid))]
        [InverseProperty("Songreports")]
        public virtual Song Song { get; set; } = null!;
    }
}
