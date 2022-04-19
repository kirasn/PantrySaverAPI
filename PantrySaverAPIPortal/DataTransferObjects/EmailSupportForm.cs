using System.ComponentModel.DataAnnotations;

namespace PantrySaverAPIPortal.DataTransferObjects
{
    public class EmailSupportForm
    {
        [Required]
        public string EmailFrom { get; set; }
        [Required]
        public string Content { get; set; }
    }
}