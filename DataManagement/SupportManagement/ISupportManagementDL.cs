using PantrySaver.Models;

namespace DataManagement.SupportManagement
{
    public interface ISupportManagementDL
    {
        Task<EmailSupport> PostNewEmail(EmailSupport emailSupport);
    }
}