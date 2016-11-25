using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EBS.Infrastructure.Events;
namespace EBS.Domain.Event
{
   public class SaleOrderSyncHander:IConsumer<SaleOrderSyncEvent>
    {
        public void HandleEvent(SaleOrderSyncEvent eventMessage)
        {
            throw new NotImplementedException();
        }
    }
}
