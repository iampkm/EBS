using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBS.Application.DTO
{
   public class AccessTokenModel
    {
        public int StoreId { get; set; }

        public int PosId { get; set; }

        public string CDKey { get; set; }
    }
}
