using DataManagement.SupportManagement;
using PantrySaver.Models;

namespace BussinessManagement.SupportManagement
{
    public class SupportManagementBL : ISupportManagementBL
    {
        private readonly ISupportManagementDL _repo;

        public SupportManagementBL(ISupportManagementDL repo)
        {
            _repo = repo;
        }

        public async Task<EmailSupport> PostNewEmail(EmailSupport emailSupport)
        {
            return await _repo.PostNewEmail(emailSupport);
        }
    }
}