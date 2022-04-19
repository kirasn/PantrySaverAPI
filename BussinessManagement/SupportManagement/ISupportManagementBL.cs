using PantrySaver.Models;

namespace BussinessManagement.SupportManagement
{
    public interface ISupportManagementBL
    {
        Task<EmailSupport> PostNewEmail(EmailSupport emailSupport);
    }
}