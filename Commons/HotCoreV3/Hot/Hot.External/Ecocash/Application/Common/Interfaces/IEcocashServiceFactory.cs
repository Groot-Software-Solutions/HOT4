using Hot.Ecocash.Domain.Enums;

namespace Hot.Ecocash.Application.Common.Interfaces
{
    public interface IEcocashServiceFactory
    {
        public EcocashAccounts GetEcocashAccountByMerchant(string merchantCode);
        public IEcocashService GetService(EcocashAccounts account);
    }
}
