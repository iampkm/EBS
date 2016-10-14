using EBS.Application.DTO;
namespace EBS.Admin.Services
{
    public interface IContextService
    {
       // AccountIdentity CurrentAccount { get; }
        AccountInfo CurrentAccount { get; }
    }

    public class AccountIdentity
    {
        public string AccountId { get; private set; }
        public string AccountName { get; private set; }

        public AccountIdentity(string accountId, string accountName)
        {
            AccountId = accountId;
            AccountName = accountName;
        }
    }
}
