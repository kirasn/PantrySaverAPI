namespace PantrySaver.Models
{
    public partial class PantryItem
    {
        public string PantryItemId { get; set; } = null!;
        public string PantryId { get; set; } = null!;
        public string ItemId { get; set; } = null!;
        public int? Quantity { get; set; }
        public DateTime? ExpiredDate { get; set; }
        public int? AlertQuantity { get; set; }
        public DateTime? AlertDate { get; set; }

        public virtual Item Item { get; set; } = null!;
        public virtual Pantry Pantry { get; set; } = null!;
    }
}