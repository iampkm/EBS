using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBS.Application.DTO
{
    /// <summary>
    /// 创建班次消息
    /// </summary>
   public class CreatedWorkScheduleMessage
    {
       public int Id { get; set; }
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
    }

    /// <summary>
    /// 输入班次收现消息
    /// </summary>
   public class InputCashAmountMessage
   { 
         private InputCashAmountMessage() { }
         public InputCashAmountMessage(int id, decimal money)
       {
           this.Id = id;
           this.Money = money;
       }

       /// <summary>
       /// 班次Id
       /// </summary>
       public int Id { get;private set; }
       /// <summary>
       /// 收现金额
       /// </summary>
       public decimal Money { get; private set; }
   }
}
