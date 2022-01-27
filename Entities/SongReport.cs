using System;
using System.Collections.Generic;

namespace _4kTiles_Backend.Entities
{
    public partial class SongReport
    {
        public int ReportId { get; set; }
        public int SongId { get; set; }
        public int AccountId { get; set; }
        public string ReportTitle { get; set; } = null!;
        public string ReportReason { get; set; } = null!;
        public DateTime ReportDate { get; set; }
        public bool? ReportStatus { get; set; }

        public virtual Account Account { get; set; } = null!;
        public virtual Song Song { get; set; } = null!;
    }
}
