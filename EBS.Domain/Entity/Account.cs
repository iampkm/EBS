using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using Dapper.DBContext;
using EBS.Infrastructure.Extension;
using EBS.Domain;
using EBS.Domain.ValueObject;
namespace EBS.Domain.Entity
{
    public class Account : BaseEntity
    {
        private const int _FailureTimes = 5;
        private const int _ResetMinute = 15;

        public Account()
        {
            this.Status = AccountStatus.Active;
            this.CreatedOn = DateTime.Now;
            this.LastUpdateDate = DateTime.Now;
        }
        public Account(string userName, string password, string nickName, int roleId,int storeId)
            : this()
        {
            this.UserName = userName;
            this.Password = password;
            this.NickName = nickName;
            this.RoleId = roleId;
            this.StoreId = storeId;        
        }

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

        public AccountStatus Status { get; private set; }
        /// <summary>
        /// 最后登录时间
        /// </summary>
        public DateTime LastUpdateDate { get; set; }
        /// <summary>
        /// 登录错误次数
        /// </summary>
        public int LoginErrorCount { get; private set; }

        /// <summary>
        ///可以查看门店的权限， 0查看所有，逗号分隔门店ID
        /// </summary>
        public string CanViewStores { get; set; }

        public void Actived()
        {
            this.Status = AccountStatus.Active;
        }
        public void Deleted()
        {
            this.Status = AccountStatus.Deleted;
        }

        public void Disabled()
        {
            this.Status = AccountStatus.Disabled;
        }
        /// <summary>
        /// 加密密码
        /// </summary>
        public void EncryptionPassword()
        {
            MD5 md5Prider = MD5.Create();
            this.Password = md5Prider.GetMd5Hash(this.Password);
        }

        public void ResetErrorCount()
        {
            this.LoginErrorCount = 0;
            this.LastUpdateDate = DateTime.Now;
        }

        public bool CheckPassword(string password)
        {
            //if (userName != this.UserName)
            //{
            //    return false;
            //}
            MD5 md5Prider = MD5.Create();
            if (!md5Prider.VerifyMd5Hash(password, this.Password))
            {
                return false;
            }
            return true;
        }

        public void CheckAccountState()
        {
            if (this.Status != AccountStatus.Active) { throw new Exception("账户未激活"); }
        }
        /// <summary>
        /// 校验登录
        /// </summary>
        public void CheckLoginFailedTimes()
        {
            if (this.LastUpdateDate.AddMinutes(_ResetMinute) < DateTime.Now)
            {   //距离最后一次登陆错误，超过15分钟，自动解锁
                this.LoginErrorCount = 0;
                this.LastUpdateDate = DateTime.Now;
            }
            if (this.LoginErrorCount >= _FailureTimes)
            {
                throw new Exception(string.Format("您登陆错误次数超过{0}次，请{1}分钟后重试!", _FailureTimes, _ResetMinute));
            }
        }
        /// <summary>
        /// 统计登录错误次数
        /// </summary>
        public bool CountLoginfailedTimes()
        {
            if (this.LoginErrorCount < _FailureTimes)
            {
                this.LoginErrorCount += 1;
                this.LastUpdateDate = DateTime.Now;
                return true;
            }
            return false;
        }
    }
}
