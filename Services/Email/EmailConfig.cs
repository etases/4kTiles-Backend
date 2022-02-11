namespace _4kTiles_Backend.Services.Email
{
    public class EmailConfig
    {
        public string SmtpServer { get; set; } = null!;
        public string MailAddress { get; set; } = null!;
        public string MailPassword { get; set; } = null!;
        public int MailPort { get; set; }
        public string DisplayName { get; set; } = "Piano Co.";
    }
}
