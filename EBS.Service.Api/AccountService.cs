using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EBS.Command;
using EBS.Domain.Accounts;
using Dapper.DBContext;
using EBS.Command.Models;
using FluentValidation.Results;
using FluentValidation;
namespace EBS.Command.Service
{
    public class AccountService : IAccountService
    {
        IDBContext _db;
       // IAccountLogic _accountLogic;
        public AccountService(IDBContext dbContext)
        {
            this._db = dbContext;
           // this._accountLogic = accountLogic;
        }       

        public AccountInfo Login(LoginModel model)
        {
            model.Validate();
            var account = this._db.Table.Find<Account>(a => a.UserName == model.UserName);
            if (account == null) throw new Exception("用户名或密码错误!");
            if (account.VerifyAccount(model.UserName, model.Password))
            {
                AccountLoginHistory loginHistory = new AccountLoginHistory(account.Id, account.UserName, model.IpAddress);
                this._db.Insert<AccountLoginHistory>(loginHistory);
                this._db.SaveChange();
                return new AccountInfo() {  AccountId = account.Id, UserName = account.UserName, RoleId = account.RoleId};
            }
            else
            {
                throw new Exception("用户名或密码错误!");
            }            
        }
    }
}
