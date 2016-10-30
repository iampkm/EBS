using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace EBS.Application.DTO
{
   public class PurchaseContractModel
    {
       public PurchaseContractModel()
       {        
           this.CreatedOn = DateTime.Now;
           this.UpdatedOn = DateTime.Now;
         
       }
       public int Id { get; set; }
        public string Code { get; set; }

        public string Name { get; set; }

        public int SupplierId { get; set; }
        public string Contact { get; set; }
        public int Cooperate { get; set; }
        /// <summary>
        /// 合同开始日期
        /// </summary>
        public DateTime StartDate { get; set; }
        /// <summary>
        /// 合同开始日期
        /// </summary>
        public DateTime EndDate { get; set; }

        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public int Status { get; set; }
       /// <summary>
       ///  id,price 键值对
       /// </summary>
        public string Items { get; set; }

        public Dictionary<int, decimal> ProductPriceDic { get; set; }

   
       
    }
}
