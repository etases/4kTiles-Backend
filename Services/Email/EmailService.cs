using System.Net.Mail;

using _4kTiles_Backend.DataObjects.DTO.Email;

namespace _4kTiles_Backend.Services.Email
{
    public interface IEmailService
    {
        Task SendEmail(EmailContent content);
    }
    public class EmailService : IEmailService
    {
        private readonly string _contentTemplate;
        private readonly EmailConfig _emailConfig;

        public EmailService(EmailConfig emailConfig, IWebHostEnvironment env)
        {
            this._emailConfig = emailConfig;

            // Read mail template
            var filePath = env.WebRootPath + "/Templates/MailTemplate.html";
            var str = new StreamReader(filePath);
            var mailTemplate = str.ReadToEnd();
            str.Close();

            _contentTemplate = mailTemplate;
        }

        /// <summary>
        /// Send the email
        /// </summary>
        /// <param name="content">the email content</param>
        /// <returns>the task</returns>
        public Task SendEmail(EmailContent content)
        {
            // Make the final content
            var replacedContent = _contentTemplate
                .Replace("[logo]", content.Logo)
                .Replace("[subject]", content.Subject)
                .Replace("[description]", content.Description)
                .Replace("[value]", content.Value)
                .Replace("[date]", content.Date.ToShortDateString())
                .Replace("[company_link]", content.CompanyLink)
                .Replace("[company_name]", content.CompanyName)
                .Replace("[company_link]", content.CompanyLink)
                .Replace("[pic]", content.AdditionalPicture)
                .Replace("[reason]", content.Reason)
                .Replace("[footer-head]", content.FooterHead)
                .Replace("[footer-content]", content.FooterContent)
                .Replace("[color1]", content.BackgroundColor)
                .Replace("[color2]", content.DescriptionColor)
                .Replace("[color3]", content.ValueColor)
                .Replace("[color4]", content.CompanyLinkTextColor)
                .Replace("[color5]", content.CompanyLinkBackgroundColor);

            // Mail service config
            var mail = new MailMessage();
            var smtpClient = new SmtpClient(_emailConfig.SmtpServer)
            {
                Port = _emailConfig.MailPort,
                Credentials = new System.Net.NetworkCredential(_emailConfig.MailAddress, _emailConfig.MailPassword),
                EnableSsl = true
            };

            // Mail content config
            mail.From = new MailAddress(_emailConfig.MailAddress, _emailConfig.DisplayName);
            mail.To.Add(content.ToEmail);
            mail.Subject = content.Subject;
            mail.IsBodyHtml = true;
            mail.Body = replacedContent;

            // Send the mail
            return smtpClient.SendMailAsync(mail);
        }
    }
}
