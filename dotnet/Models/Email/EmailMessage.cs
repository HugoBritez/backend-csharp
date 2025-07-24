namespace Api.Models.Email
{
    public class EmailMessage
    {
        public string To { get; set; } = string.Empty;
        public string[]? ToMultiple { get; set; }
        public string Subject { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
        public bool IsHtml { get; set; }
        public string? AttachmentPath { get; set; }
        public byte[]? AttachmentData { get; set; }
        public string? AttachmentName { get; set; }
    }
}