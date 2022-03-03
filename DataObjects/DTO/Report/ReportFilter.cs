using System.ComponentModel.DataAnnotations;

namespace _4kTiles_Backend.DataObjects.DTO.Report
{
    public class ReportFilter
    {
        [Required]
        public string? Status { get; set; }
        [Required]
        public string? Title { get; set; }
    }
}
