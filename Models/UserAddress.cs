namespace PantrySaver.Models
{
    public partial class UserAddress
    {
        public string UserAddressId { get; set; } = null!;
        public string UserId { get; set; } = null!;
        public string? AddressName { get; set; }
        public string Address1 { get; set; } = null!;
        public string? Address2 { get; set; }
        public string? Address3 { get; set; }
        public string City { get; set; } = null!;
        public string State { get; set; } = null!;
        public string Zipcode { get; set; } = null!;
        public string Country { get; set; } = null!;

        public virtual ApplicationUser? User { get; set; }
    }
}