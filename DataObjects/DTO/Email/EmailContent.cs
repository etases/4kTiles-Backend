namespace _4kTiles_Backend.DataObjects.DTO.Email
{
    public class EmailContent
    {
        #region Required Email Settings
        public string ToEmail { get; set; } = null!;
        public string ShortSubject { get; set; } = null!;
        #endregion

        #region Email Body
        public string Logo { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
        public DateTime Date { get; set; } = DateTime.Now;
        public string CompanyLink { get; set; } = string.Empty;
        public string CompanyName { get; set; } = string.Empty;
        public string AdditionalPicture { get; set; } = string.Empty;
        public string Reason { get; set; } = string.Empty;
        public string FooterHead { get; set; } = string.Empty;
        public string FooterContent { get; set; } = string.Empty;
        #endregion

        #region Color
        public string BackgroundColor { get; set; } = "#2eb2d5";
        public string DescriptionColor { get; set; } = "#000000";
        public string ValueColor { get; set; } = "#ffdf01";
        public string CompanyLinkTextColor { get; set; } = "#ffffff";
        public string CompanyLinkBackgroundColor { get; set; } = "#2eb2d5";
        #endregion
    }
}
