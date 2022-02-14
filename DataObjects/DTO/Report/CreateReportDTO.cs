using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace _4kTiles_Backend.DataObjects.DTO.Report
{
    public class CreateReportDTO
    {
        [Required]
        public int ReportId { get; set; }
        [Required]
        public int SongId { get; set; }
        [Required]
        public int AccountId { get; set; }
        [Required]
        public string ReportTitle { get; set; }
        [Required]
        public string ReportReason { get; set; }
        [Required]
        public DateTime ReportDate { get; set; }
    }
}
