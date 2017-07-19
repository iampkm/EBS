using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBS.Application.DTO
{
   public class OutInOrderModel
    {
        public int Id { get; set; }
        public int StoreId { get; set; }

        public int SupplierId { get; set; }

        public int OutInOrderTypeId { get; set; }
        public string Code { get; set; }

        public int EditBy { get; set; }

        public string EditByName { get; set; }

        /// <summary>
        /// 明细 json 串
        /// </summary>
       public string ItemsJson { get; set; }

       public bool SaveAndSubmit { get; set; }
    }
}
