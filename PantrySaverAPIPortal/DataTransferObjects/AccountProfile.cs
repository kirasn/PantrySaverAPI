namespace PantrySaverAPIPortal.DataTransferObjects
{
    public class AccountProfile
    {
        public string? UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? Email { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime? RegistrationDate { get; set; }
    }
}