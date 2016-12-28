using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBS.Application.DTO
{
   public class TransferOrderModel
    {
       public int Id { get; set; }
        public int FromStoreId { get; set; }
        public string FromStoreName { get; set; }
        public int ToStoreId { get; set; }
        public string ToStoreName { get; set; }
        public string Code { get; set; }

        public string StatusName { get; set; }

        public int EditBy { get; set; }

        public string EditByName { get; set; }

       /// <summary>
       /// 明细 json 串
       /// </summary>
        public string ItemsJson { get; set; }
    }
}
