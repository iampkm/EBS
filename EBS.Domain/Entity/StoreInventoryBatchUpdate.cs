using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBS.Domain.Entity
{
   public class StoreInventoryBatchUpdate:BaseEntity
    {
        //public StoreInventoryBatchUpdate(int id, int quantity)
        //{
        //    this.Id = id;
        //    this.Quantity = quantity;
        //}
        //public StoreInventoryBatchUpdate(int id, int quantity,int sourceQuantity):this(id,quantity)
        //{           
        //    this.SourceQuantity = sourceQuantity;
        //}
       /// <summary>
       /// 新库存数
       /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// 原库存数
        /// </summary>
        public int SourceQuantity { get; set; }
    }

   
}
