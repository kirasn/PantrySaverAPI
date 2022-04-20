using System.ComponentModel.DataAnnotations;

namespace PantrySaverAPIPortal.DataTransferObjects
{
    public class NewPantryForm
    {
        [Required]
        public string PantryName { get; set; }
        [Required]
        public string Location { get; set; }
    }
}