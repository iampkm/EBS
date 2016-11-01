using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace EBS.Application.DTO
{
    public class CreatePurchaseContract
    {  
        public string Code { get; set; }

        public string Name { get; set; }

        public int StoreId { get; set; }
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
        public int CreatedBy { get; set; }

        public string CreatedByName { get; set; }
       /// <summary>
       ///  id,price 键值对
       /// </summary>
        public string Items { get; set; }
        public Dictionary<int, decimal> ProductPriceDic { get; set; }
       
    }

   public class EditPurchaseContract
   {
       public int Id { get; set; }
       public string Code { get; set; }

       public string Name { get; set; }
        public int StoreId { get; set; }

       public int SupplierId { get; set; }
       public string Contact { get; set; }
       public int Status { get; set; }

       /// <summary>
       /// 合同开始日期
       /// </summary>
       public DateTime StartDate { get; set; }
       /// <summary>
       /// 合同开始日期
       /// </summary>
       public DateTime EndDate { get; set; }
       public int UpdatedBy { get; set; }

       public string UpdatedByName { get; set; }
       /// <summary>
       ///  id,price 键值对
       /// </summary>
       public string Items { get; set; }
       public Dictionary<int, decimal> ProductPriceDic { get; set; }

   }
}
