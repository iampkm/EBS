using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper.DBContext;
namespace EBS.Domain.Accounts
{
   public class AccountLoginHistory : AggregateRoot<int>
    {
       public AccountLoginHistory(int accountId,string userName, string ipAddress,LoginStatus status = Accounts.LoginStatus.Login)
       {
           this.AccountId = accountId;
           this.UserName = userName;
           this.IPAddress = ipAddress;
           this.LoginStatus = status.ToString();
           this.CreatedOn = DateTime.Now;
       }
       public int AccountId { get; private set; }

       public string UserName { get; private set; }

       public DateTime CreatedOn { get; private set; }

       public string IPAddress { get; private set; }

       public string LoginStatus { get; private set; }
    }
}
