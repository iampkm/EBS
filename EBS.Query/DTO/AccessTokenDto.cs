using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBS.Query.DTO
{
    /// <summary>
    /// 访问token
    /// </summary>
   public class AccessTokenDto
    {
       public int StoreId { get; set; }

       public int PosId { get; set; }

       public string CDKey { get; set; }
    }
}
