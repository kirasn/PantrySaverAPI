namespace PantrySaverAPIPortal.Consts
{
    public class RouteConfigs
    {
        // Routing: Authentication/
        public const string Register = "Register";
        public const string Login = "Login";
        public const string ChangePassword = "ChangePassword";
        public const string EmailConfirmation = "EmailConfirmation";
        public const string TwoFactorAuthentication = "TwoFactorAuthentication";
        public const string GenerateNewEmailConfirmation = "GenerateNewEmailConfirmation";

        // Routing: Account/
        public const string Profile = "Profile";

        // Routing: Support/
        public const string Email = "Email";

        // Routing: Pantry/
        public const string Pantries = "Pantries";
        public const string PantryDetails = "Pantries/{pantryId}";

        // Routing: Item/
        public const string Items = "Items";
    }
}