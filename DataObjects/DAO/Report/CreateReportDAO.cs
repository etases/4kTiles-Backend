namespace _4kTiles_Backend.DataObjects.DAO.Report
{
    public class CreateReportDAO
    {
        public int ReportId { get; set; }
        public int SongId { get; set; }
        public int AccountId { get; set; }
        public string ReportTitle { get; set; } = null!;
        public string ReportReason { get; set; } = null!;
        public DateTime ReportDate { get; set; }
    }
}