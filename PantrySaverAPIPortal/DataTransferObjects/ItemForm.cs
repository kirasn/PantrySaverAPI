using System.ComponentModel.DataAnnotations;

namespace PantrySaverAPIPortal.DataTransferObjects
{
    public class ItemForm
    {
        [Required]
        public string Name { get; set; } = null!;
        public string? BarcodeFormats { get; set; }
        public string? Category { get; set; }
        public string? Manufacturer { get; set; }
        public string? ImageUrl { get; set; }
        public string? Description { get; set; }
    }
}