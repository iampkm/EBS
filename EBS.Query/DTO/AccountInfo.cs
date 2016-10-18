using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EBS.Domain.ValueObject;
using EBS.Infrastructure.Extension;
namespace EBS.Query.DTO
{
    public class AccountInfo
    {
        public int Id { get; set; }

        public string UserName { get; set; }

        public string NickName { get; set; }
        /// <summary>
        /// 角色名
        /// </summary>
        public string RoleName { get; set; }
        public string CreatedOn { get; set; }

        public AccountStatus Status { get; set; }

        public string StatusName
        {
            get
            {
                return Status.Description();
            }
        }       
        /// <summary>
        /// 登录错误次数
        /// </summary>
        public int LoginErrorCount { get; set; }
    }
}
