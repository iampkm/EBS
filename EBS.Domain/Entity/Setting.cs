using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBS.Domain.Entity
{
    /// <summary>
    /// 设置表
    /// </summary>
   public class Setting:BaseEntity
    {
         public string KeyTitle { get; set; }

        public string KeyName { get; set; }
        public string KeyType { get; set; }
        public string ValueTitle { get; set; }
        public string Value { get; set; }
        /// <summary>
        /// 门店Id ， 为0 表示通用配置
        /// </summary>
        public int StoreId  { get; set; }
        /// <summary>
        ///  显示顺序
        /// </summary>
        public int DisplayOrder { get; set; }
    }
}
