using PantrySaver.Models;

namespace DataManagement.SupportManagement
{
    public class SupportManagementDL : ISupportManagementDL
    {
        private readonly PantrySaverContext _context;

        public SupportManagementDL(PantrySaverContext context)
        {
            _context = context;
        }

        public async Task<EmailSupport> PostNewEmail(EmailSupport emailSupport)
        {
            emailSupport.CreatedAt = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time"));
            await _context.EmailSupports.AddAsync(emailSupport);
            await _context.SaveChangesAsync();

            return emailSupport;
        }
    }
}