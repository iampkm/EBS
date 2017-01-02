using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EBS.Application.Message;
namespace EBS.Application
{
    /// <summary>
    /// Pos 收银数据同步
    /// </summary>
   public interface IPosSyncFacade
    {
       void SaleOrderSync(string body);
       void WorkScheduleSync(string body);

        void UpdateSaleSync(string body);
    }
}
