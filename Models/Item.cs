namespace PantrySaver.Models
{
    public partial class Item
    {
        public Item()
        {
            PantryItems = new HashSet<PantryItem>();
        }

        public string ItemId { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string? BarcodeFormats { get; set; }
        public string? Category { get; set; }
        public string? Manufacturer { get; set; }
        public string? ImageUrl { get; set; }
        public string? Description { get; set; }
        public bool IsCustom { get; set; }

        public virtual ICollection<PantryItem> PantryItems { get; set; }
    }
}