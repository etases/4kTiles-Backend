using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace _4kTiles_Backend.Entities
{
    [Table("SongReport")]
    public partial class SongReport
    {
        [Key]
        [Column("reportId")]
        public int ReportId { get; set; }
        [Column("songId")]
        public int SongId { get; set; }
        [Column("accountId")]
        public int AccountId { get; set; }
        [Column("reportTitle")]
        [StringLength(255)]
        public string ReportTitle { get; set; } = null!;
        [Column("reportReason")]
        [StringLength(255)]
        public string ReportReason { get; set; } = null!;
        [Column("reportDate", TypeName = "datetime")]
        public DateTime ReportDate { get; set; }
        [Column("reportStatus")]
        public bool? ReportStatus { get; set; }

        [ForeignKey(nameof(AccountId))]
        [InverseProperty("SongReports")]
        public virtual Account Account { get; set; } = null!;
        [ForeignKey(nameof(SongId))]
        [InverseProperty("SongReports")]
        public virtual Song Song { get; set; } = null!;
    }
}
