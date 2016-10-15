using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBS.Query.DTO
{
   public class Pager
    {
       /// <summary>
       /// 当前页索引
       /// </summary>
       public int PageIndex { get; set; }
       /// <summary>
       /// 每页显示行数
       /// </summary>
       public int PageSize { get; set; }
       /// <summary>
       /// 总记录数
       /// </summary>
       public int Total { get; set; }
       /// <summary>
       /// 是否分页
       /// </summary>
       public bool IsPaging { get; set; }
    }
}
