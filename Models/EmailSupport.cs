namespace PantrySaver.Models
{
    public class EmailSupport
    {
        public string EmailSupportId { get; set; }
        public string EmailFrom { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? Answer { get; set; }
        public DateTime? ResponsedAt { get; set; }
    }
}