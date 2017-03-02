using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBS.Query.DTO
{
   public class AccessTokenDto
    {
       public int Id { get; set; }

       public string StoreName { get; set; }

       public int PosId { get; set; }

       public string CDKey { get; set; }
    }
}
