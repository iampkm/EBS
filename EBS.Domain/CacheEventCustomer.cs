using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EBS.Infrastructure.Caching;
using EBS.Infrastructure.Events;
using EBS.Domain.Accounts;
namespace EBS.Domain
{
    public class CacheEventCustomer: IConsumer<Account>
    {
        public void HandleEvent(Account eventMessage)
        {
            throw new NotImplementedException();
        }
    }
}
