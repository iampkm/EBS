using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBS.Query.DTO
{
    public class StoreDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Contact { get; set; }
        public string Phone { get; set; }
        /// <summary>
        /// 日期字段设置为字符串，viewtable 中就可以显示了
        /// </summary>
        public string CreatedOn { get; set; }
    }
}
