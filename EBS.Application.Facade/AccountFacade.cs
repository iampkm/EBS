using System;
using EBS.Domain.Entity;
using EBS.Domain.Service;
using Dapper.DBContext;
using EBS.Application;
using EBS.Application.DTO;
namespace EBS.Command.Service
{
    public class AccountFacade : IAccountFacade 
    {
        IDBContext _db;
        AccountService _accountService;
        public AccountFacade(IDBContext dbContext)
        {
            this._db = dbContext;
            _accountService = new AccountService(this._db);
        
        }       

        public AccountInfo Login(LoginModel model)
        {
            model.Validate();
            var account = _accountService.CheckAccount(model.UserName, model.Password, model.IpAddress);
            return new AccountInfo() { AccountId = account.Id, UserName = account.UserName, RoleId = account.RoleId };
        }
    }
}
