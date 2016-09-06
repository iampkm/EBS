using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBS.Domain.Accounts
{
    public enum AccountStatus
    {
        /// <summary>
        /// 激活
        /// </summary>
        Active = 1,
        /// <summary>
        /// 删除
        /// </summary>
        Deleted = 2,
        /// <summary>
        /// 禁用
        /// </summary>
        Disabled = 3
    }
}
