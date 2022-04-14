using System.ComponentModel.DataAnnotations;

namespace PantrySaverAPIPortal.DataTransferObjects
{
    public class TwoFactorAuthenticationForm
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Provider { get; set; }
        [Required]
        public string Token { get; set; }
    }
}