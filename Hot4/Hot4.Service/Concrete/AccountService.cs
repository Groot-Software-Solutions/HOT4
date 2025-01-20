using AutoMapper;
using Hot4.DataModel.Models;
using Hot4.Repository.Abstract;
using Hot4.Service.Abstract;
using Hot4.ViewModel;

namespace Hot4.Service.Concrete
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IMapper Mapper;
        public AccountService(IAccountRepository accountRepository, IMapper mapper)
        {
            _accountRepository = accountRepository;
            Mapper = mapper;
        }
        public async Task<bool> AddAccount(AccountModel account)
        {
            var model = Mapper.Map<Account>(account);
            return await _accountRepository.AddAccount(model);
        }

        public async Task<bool> DeleteAccount(long accountId)
        {
            var record = await GetEntityById(accountId);
            if (record != null)
            {
                return await _accountRepository.DeleteAccount(record);
            }
            return false;
        }

        public async Task<AccountModel?> GetAccountById(long accountId)
        {
            var record = await GetEntityById(accountId);
            return Mapper.Map<AccountModel>(record);
        }

        public async Task<ViewAccountModel?> GetAccountDetailById(long accountId)
        {
            return await _accountRepository.GetAccountDetailById(accountId);
        }

        public async Task<List<ViewAccountModel>> SearchAccount(string filter, int pageNo, int pageSize)
        {
            return await _accountRepository.SearchAccount(filter, pageNo, pageSize);
        }

        public async Task<bool> UpdateAccount(AccountModel account)
        {
            var record = await GetEntityById(account.AccountId);
            if (record != null)
            {
                Mapper.Map(account, record);
                return await _accountRepository.UpdateAccount(record);
            }
            return false;
        }
        private async Task<Account?> GetEntityById(long accountId)
        {
            return await _accountRepository.GetAccountById(accountId);
        }
    }
}
