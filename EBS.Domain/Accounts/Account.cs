using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using Dapper.DBContext;
using EBS.Infrastructure.Extension;
namespace EBS.Domain.Accounts
{
    public class Account : AggregateRoot<int>
    {
        public Account() {
            this.Status = AccountStatus.Active;
            this.CreatedOn = DateTime.Now;
        }
        public Account(string userName, string password, string nickName, int roleId):this()
        {
            this.UserName = userName;
            this.Password = password;
            this.NickName = nickName;
            this.RoleId = roleId;
        }

        public string UserName { get; private set; }
        /// <summary>
        /// MD5 加密密码
        /// </summary>
        public string Password { get; private set; }

        public string NickName { get; private set; }

        public DateTime CreatedOn { get; private set; }

        public AccountStatus Status { get; private set; }

        public int RoleId { get; private set; }

        public void Deleted()
        {
            this.Status = AccountStatus.Deleted;
        }

        public void Disabled()
        {
            this.Status = AccountStatus.Disabled;
        }

        public bool VerifyAccount(string userName, string password)
        {
            if (this == null) { return false; }
            if (userName != this.UserName)
            {
                return false;
            }
            MD5 md5Prider = MD5.Create();
            if (!md5Prider.VerifyMd5Hash(password,this.Password))
            {
                return false;
            }
            return true;
        }
    }
}
