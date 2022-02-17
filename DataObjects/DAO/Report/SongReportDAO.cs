namespace _4kTiles_Backend.DataObjects.DAO.Report
{
    public class SongReportDAO
    {
        public int ReportId { get; set; }
        public int SongId { get; set; }
        public int AccountId { get; set; }
        public string ReportTitle { get; set; } = null!;
        public string ReportReason { get; set; } = null!;
        public DateTime ReportDate { get; set; }
        public bool? ReportStatus { get; set; }
    }
}
