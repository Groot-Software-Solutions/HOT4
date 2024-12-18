using Hot4.Core.DataViewModels;
using Hot4.DataModel.Models;
using Hot4.Repository.Abstract;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Hot4.Server.Components.Pages
{
    public class AccessGetModel : PageModel
    {
        private IAccessRepository _accessRepository;
        public Access access = null;
        public List<Access> accessesLst = null;
        public long getAdminId = 0;
        public AccountAccessModel accountAccessModel = null;

        public AccessGetModel(IAccessRepository accessRepository)
        {
            _accessRepository = accessRepository;
        }
        public async void GetByAccessId()
        {
            long accessId = 10475953;
            access = await _accessRepository.GetAccess(accessId);

        }

        public async void ListAccountChannel()
        {
            accessesLst = new List<Access>();
            long accountId = 10000564;
            byte channelId = 1;
            accessesLst = await _accessRepository.ListAccountChannel(accountId, channelId);
        }
        public async void GetByAccessCode()
        {
            accountAccessModel = new AccountAccessModel();
            string accessCode = "0773565417";
            accountAccessModel = await _accessRepository.GetByAccessCode(accessCode);
        }

        public async void GetAdminId()
        {
            getAdminId = 0;
            getAdminId = await _accessRepository.GetAdminId(10000564);

        }

        public async void GetLoginDetails()
        {
            accountAccessModel = new AccountAccessModel();

            string accessCode = "7009373728";
            string accessPassword = "5555";
            accountAccessModel = await _accessRepository.GetLoginDetails(accessCode, accessPassword);
        }

        public async void SaveAccess()
        {
            var mdl = new Access() { AccessCode = "7009373728", AccessPassword = "5555", AccountId = 10001002, ChannelId = 1 };
            await _accessRepository.AddAccess(mdl);

        }
    }
}
