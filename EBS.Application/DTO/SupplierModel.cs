using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBS.Application.DTO
{
   public class SupplierModel
    {
       public int Id { get; set; }
        public string Name { get; set; }

        public string Code { get; set; }
        /// <summary>
        /// 简称
        /// </summary>
        public string ShortName { get; set; }
        /// <summary>
        /// 联系人
        /// </summary>
        public string Contact { get; set; }
        public string QQ { get; set; }

        public string Address { get; set; }
        /// <summary>
        /// 电话
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// 开户行
        /// </summary>
        public string Bank { get; set; }
        /// <summary>
        /// 税号
        /// </summary>
        public string TaxNo { get; set; }
        /// <summary>
        /// 开户行账号
        /// </summary>
        public string BankAccount { get; set; }
        /// <summary>
        /// 执照编号
        /// </summary>
        public string LicenseNo { get; set; }

        /// <summary>
        /// 供货区域
        /// </summary>
        public string AreaId { get; set; }
       /// <summary>
       /// 编辑人
       /// </summary>
        public int editedBy { get; set; }
        /// <summary>
        /// 编辑人名
        /// </summary>
        public string editor { get; set; }

        public int Type { get; set; }
    }
}
