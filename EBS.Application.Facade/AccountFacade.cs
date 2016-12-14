using System;
using EBS.Domain.Entity;
using EBS.Domain.Service;
using Dapper.DBContext;
using EBS.Application;
using EBS.Application.DTO;
using EBS.Application.Facade.Mapping;
namespace EBS.Application.Service
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
            var role = _db.Table.Find<Role>(account.RoleId);
            var storeName = "总公司";
            if (account.StoreId > 0)
            {
                storeName = _db.Table.Find<Store>(account.StoreId).Name;
            }
            return new AccountInfo() { 
                AccountId = account.Id, UserName = account.UserName,
                RoleId = account.RoleId ,NickName = account.NickName,
                StoreId = account.StoreId, RoleName = role.Name,
                StoreName = storeName,
                CanViewStores = account.CanViewStores
            };
        }


        public void Create(CreateAccountModel model)
        {           
            Account entity = new Account(model.UserName, model.Password, model.NickName, model.RoleId,model.StoreId);
            entity.CanViewStores = model.CanViewStores;
            _accountService.Create(entity);
            _db.SaveChange();
        }

        public void Edit(EditAccountModel model)
        {
            //Account entity = new Account()
            //{
            //    Id = model.Id,
            //    NickName = model.NickName,
            //    RoleId = model.RoleId
            //};
            Account entity = _db.Table.Find<Account>(model.Id);
            entity = model.MapTo<Account>(entity);
            entity.LastUpdateDate = DateTime.Now;
            // _accountService.Update(entity);
            _db.Update(entity);
            _db.SaveChange();
        }

        public void ChangePassword(int id, string oldPassword, string newPassword)
        {
              
            _accountService.ChangePassword(id, oldPassword, newPassword);
            _db.SaveChange();
        }

        public void ResetPassword(int id)
        {
            _accountService.ResetPassword(id);
            _db.SaveChange();
        }

        public void ActiveAccount(int id)
        {
            var entity = _db.Table.Find<Account>(id);
            entity.Actived();
            _db.Update(entity);
            _db.SaveChange();
        }

        public void DisabledAccount(int id)
        {
            var entity = _db.Table.Find<Account>(id);
            entity.Disabled();
            _db.Update(entity);
            _db.SaveChange();
        }
    }
}
