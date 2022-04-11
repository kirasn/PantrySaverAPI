using Microsoft.AspNetCore.Identity;

namespace PantrySaver.Models
{
    public partial class ApplicationUser : IdentityUser<string>
    {
        public ApplicationUser()
        {
            Addresses = new HashSet<UserAddress>();
            PantryOwns = new HashSet<PantryOwn>();
        }

        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTime DateOfBirth { get; set; }

        public virtual ICollection<UserAddress> Addresses { get; set; }
        public virtual ICollection<PantryOwn> PantryOwns { get; set; }
    }
}