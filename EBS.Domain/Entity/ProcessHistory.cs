using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBS.Domain.Entity
{
   public class ProcessHistory:BaseEntity
    {
        public int CreatedBy { get; set; }
        public string CreatedByName { get; set; }
        public DateTime CreatedOn { get; set; }
       /// <summary>
       /// 状态
       /// </summary>
        public int Status { get; set; }
       /// <summary>
       /// 表单Id
       /// </summary>
        public int FormId { get; set; }
        /// <summary>
        /// 表单类型
        /// </summary>
        public string FormType { get; set; }
        /// <summary>
        /// 操作备注
        /// </summary>
        public string Remark { get; set; }
    }
}
