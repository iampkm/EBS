using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBS.Query.SyncObject
{
    public class AccountSync
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        /// <summary>
        /// MD5 加密密码
        /// </summary>
        public string Password { get; set; }

        public string NickName { get; set; }

        public int RoleId { get; set; }
        /// <summary>
        /// 门店，值为 0
        /// </summary>
        public int StoreId { get; set; }
        public DateTime CreatedOn { get; private set; }

        public int Status { get; private set; }
    }
}
