namespace PantrySaver.Models
{
    public partial class PantryOwn
    {
        public string PantryOwnId { get; set; } = null!;
        public string UserId { get; set; } = null!;
        public string PantryId { get; set; } = null!;
        public string Role { get; set; } = null!;

        public virtual ApplicationUser User { get; set; } = null!;
        public virtual Pantry Pantry { get; set; } = null!;
    }
}