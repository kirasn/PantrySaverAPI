namespace PantrySaver.Models
{
    public partial class Pantry
    {
        public Pantry()
        {
            PantryItems = new HashSet<PantryItem>();
            PantryOwns = new HashSet<PantryOwn>();
        }
        public string PantryId { get; set; } = null!;
        public string PantryName { get; set; } = null!;
        public string Location { get; set; } = null!;
        public Boolean IsDeleted { get; set; }

        public virtual ICollection<PantryItem> PantryItems { get; set; }
        public virtual ICollection<PantryOwn> PantryOwns { get; set; }
    }
}