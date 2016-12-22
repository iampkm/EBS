using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBS.Domain.Entity
{
   public class WorkSchedule:BaseEntity
    {
        public WorkSchedule()
        {
            this.StartDate = DateTime.Now;
           // Code = Guid.NewGuid().ToString().Replace("-", "");
        }
        public string Code { get;  set; }
        public int StoreId { get; set; }

        public int PosId { get; set; }

        public decimal CashAmount { get; set; }
        /// <summary>
        /// 上班人
        /// </summary>
        public int CreatedBy { get; set; }
        public string CreatedByName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        /// <summary>
        /// 交班人
        /// </summary>
        public int EndBy { get; set; }
        public string EndByName { get; set; }

        public void SetCashAmount(decimal amount)
        {
            this.CashAmount = amount;
        }
    }
}
