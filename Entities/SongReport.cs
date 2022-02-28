using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace _4kTiles_Backend.Entities
{
    [Table("songreport")]
    public partial class SongReport
    {
        [Key]
        [Column("reportid")]
        public int ReportId { get; set; }
        [Column("songid")]
        public int SongId { get; set; }
        [Column("accountid")]
        public int AccountId { get; set; }
        [Column("reporttitle")]
        [StringLength(255)]
        public string ReportTitle { get; set; } = null!;
        [Column("reportreason")]
        [StringLength(255)]
        public string ReportReason { get; set; } = null!;
        [Column("reportdate", TypeName = "timestamp without time zone")]
        public DateTime ReportDate { get; set; }
        [Column("reportstatus")]
        public bool? ReportStatus { get; set; }

        [ForeignKey(nameof(AccountId))]
        [InverseProperty("SongReports")]
        public virtual Account Account { get; set; } = null!;
        [ForeignKey(nameof(SongId))]
        [InverseProperty("SongReports")]
        public virtual Song Song { get; set; } = null!;
    }
}
