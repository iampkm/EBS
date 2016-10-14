using System.Web;
using System.Web.Security;
using Newtonsoft.Json;
using EBS.Application.DTO;
namespace EBS.Admin.Services
{
    public class ContextService : IContextService
    {
        public AccountInfo CurrentAccount
        {
            get
            {
                var user = HttpContext.Current.User;
                if (user != null &&
                    user.Identity != null &&
                    user.Identity.IsAuthenticated &&
                    user.Identity is FormsIdentity)
                {
                   // var accountId = ((FormsIdentity)user.Identity).Ticket.UserData;
                    var accountInfo = ((FormsIdentity)user.Identity).Ticket.UserData;
                    var account= JsonConvert.DeserializeObject<AccountInfo>(accountInfo); 
                   // var accountName = user.Identity.Name;
                  //  return new AccountIdentity(accountId, accountName);
                    return account;
                }
                return null;
            }
        }
    }
}