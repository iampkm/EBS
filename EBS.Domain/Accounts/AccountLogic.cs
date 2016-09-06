using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper.DBContext;
using System.Security.Cryptography;
using EBS.Infrastructure.Extension;
namespace EBS.Domain.Accounts
{
    public class AccountLogic : IAccountLogic
    {
        IDBContext _db;
        public AccountLogic(IDBContext dbcontext)
        {
            this._db = dbcontext;
        }

        public bool VerifyAccount(string userName, string password)
        {
            var account = this._db.Table.Find<Account>(a => a.UserName == userName);
            if (account == null) { return false; }
            if (userName != account.UserName)
            {
                return false;
            }
            MD5 md5Prider = MD5.Create();
            if (!md5Prider.VerifyMd5Hash(password, account.Password))
            {
                return false;
            }
            return true;
        }       
    }
}
