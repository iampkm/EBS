using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EBS.Domain.ValueObject;
using EBS.Infrastructure.Extension;
namespace EBS.Query.DTO
{
   public class PurchaseContractDto
    {
       public int Id { get; set; }
        public string Code { get; set; }

        public string Name { get; set; }

        public string SupplierName { get; set; }
        public string Contact { get; set; }
        public CooperateWay Cooperate { get; set; }

        public string CooperateWay {
            get {
                return Cooperate.Description();
            }
        }
        /// <summary>
        /// 合同开始日期
        /// </summary>
        public string StartDate { get; set; }
        /// <summary>
        /// 合同开始日期
        /// </summary>
        public string EndDate { get; set; }       
        public PurchaseContractStatus Status { get; set; }

        public string PurchaseContractStatus
        {
            get
            {
                return Status.Description();
            }
        }
    }
}
