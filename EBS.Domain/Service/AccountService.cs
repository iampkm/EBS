using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper.DBContext;
using System.Security.Cryptography;
using EBS.Infrastructure.Extension;
using EBS.Domain.Entity;
using System.Text.RegularExpressions;
namespace EBS.Domain.Service
{
    public class AccountService
    {
        IDBContext _db;
        public AccountService(IDBContext dbcontext)
        {
            this._db = dbcontext;
        }

        public Account CheckAccount(string userName, string password, string ipAddress)
        {
            int accountId = 0;
            int.TryParse(userName, out accountId);

            var account = this._db.Table.Find<Account>(a => a.UserName == userName || a.Id == accountId);
            if (account == null) throw new Exception("用户名或密码错误!");
            account.CheckAccountState();
            account.CheckLoginFailedTimes();
            if (account.CheckPassword(password))
            {
                this._db.Update(account);
                this._db.Insert(new AccountLoginHistory(account.Id, account.UserName, ipAddress));
                this._db.SaveChange();
                return account;
            }
            else
            {
                if (account.CountLoginfailedTimes())
                {
                    this._db.Update(account);
                    this._db.SaveChange();
                }
                throw new Exception("用户名或密码错误!");
            }
        }

        public string GenerateNewAccount()
        {
            var startNumber = 600000;
            var lastAccount= _db.Table.Find<Account>("select * from Account order by Id desc limit 1",null);
            var account = startNumber.ToString();
            if (lastAccount != null)
            {
                int accountNumber = 0;
                if (int.TryParse(lastAccount.UserName, out accountNumber) && accountNumber >= startNumber)
                {
                    accountNumber = accountNumber + 1;
                    //跳过尾数为4的数字
                    Regex regex = new Regex(@"^\d*[4]$");
                    if(regex.IsMatch(accountNumber.ToString()))
                    {
                        accountNumber = accountNumber+1;  
                    }
                    account = accountNumber.ToString();
                }               
            }
            return account;
        }

        public void ChangePassword(int id, string oldPassword, string newPassword)
        {
            Account entity = _db.Table.Find<Account>(id);
            if (entity == null) throw new Exception("账号不存在");
            if (!entity.CheckPassword(oldPassword))
            {
                throw new Exception("原密码不正确!");
            }
            entity.Password = newPassword;
            entity.EncryptionPassword();
            entity.LastUpdateDate = DateTime.Now;
            _db.Update(entity);
        }

        public void ResetPassword(int id)
        {
            Account entity = _db.Table.Find<Account>(id);
            if (entity == null) throw new Exception("账号不存在");
            entity.Password = "123456";
            entity.EncryptionPassword();
            entity.LastUpdateDate = DateTime.Now;
            _db.Update(entity);
        }
    }
}
