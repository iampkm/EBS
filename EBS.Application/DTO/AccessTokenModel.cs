using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBS.Application.DTO
{
   public class AccessTokenModel 
    {
       public int Id { get; set; }
        public int StoreId { get; set; }

        public int PosId { get; set; }
        
        /// <summary>
        /// 服务端cdkey
        /// </summary>
        public string CDKey { get; set; }
       /// <summary>
       /// pos端cdkey
       /// </summary>
        public string PosCDKey { get; set; }
        /// <summary>
        /// pos端密码
        /// </summary>
        public string PosPassword { get; set; }
    }
}
