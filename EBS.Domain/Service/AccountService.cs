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

        public void Create(Account model)
        {
            if (_db.Table.Exists<Account>(n => n.UserName == model.UserName))
            {
                throw new Exception("名称重复!");
            }
            model.Password = "123456";  //初始密码123456
            //加密密码
            model.EncryptionPassword();
            _db.Insert(model);
        }

        public void Update(Account model)
        {
            Account entity = _db.Table.Find<Account>(n => n.Id == model.Id);
            entity.NickName = model.NickName;
            entity.RoleId = model.RoleId;
            entity.LastUpdateDate = DateTime.Now;
            _db.Update(entity);
        }

        public void ChangePassword(int id, string oldPassword, string newPassword)
        {
            Account entity = _db.Table.Find<Account>(n => n.Id == id);
            if (!entity.CheckAccountAndPassword(entity.UserName, oldPassword))
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
            Account entity = _db.Table.Find<Account>(n => n.Id == id);
            entity.Password = "123456";
            entity.EncryptionPassword();
            entity.LastUpdateDate = DateTime.Now;
            _db.Update(entity);
        }
    }
}
