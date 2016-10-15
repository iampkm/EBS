using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper.DBContext;
using System.Security.Cryptography;
using EBS.Infrastructure.Extension;
using EBS.Domain.Entity;
namespace EBS.Domain.Service
{
    public class AccountService
    {
        IDBContext _db;
        public AccountService(IDBContext dbcontext)
        {
            this._db = dbcontext;
        }

        public Account CheckAccount(string userName,string password,string ipAddress)
        {
            var account = this._db.Table.Find<Account>(a => a.UserName == userName);
            if (account == null) throw new Exception("用户名或密码错误!");
            account.CheckAccountState();
            account.CheckLoginFailedTimes();
            if (account.CheckAccountAndPassword(userName, password))
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

        public void Create(Account account)
        { 
            
        }

       
    }
}
