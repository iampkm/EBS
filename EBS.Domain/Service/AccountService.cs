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

        public void Create(Account model)
        {
            //自动生成一个工号,门店编码+2位顺序号
            var preAccount = _db.Table.Find<Account>("select * from Account where StoreId=@StoreId order by UserName desc limit 1",
                new { StoreId = model.StoreId });
            var store = _db.Table.Find<Store>(n => n.Id == model.StoreId);
            var firstName = store == null ? "1000" : store.Code;
            var lastName = "";
            if (preAccount == null)
            {
                lastName = "01";
            }
            else {
                //总长度6位，取末尾两位
                var maxNumber = 0;
                int.TryParse(preAccount.UserName.Substring(4, 2), out maxNumber);               
                maxNumber = maxNumber + 1;
                if (maxNumber > 99) throw new Exception("当前门店账号已经申请满了");
                lastName = maxNumber.ToString().PadLeft(2, '0');
            }
            model.UserName = firstName + lastName;
            // 如果账号属于门店，门店查看权限自少包含自身门店ID
            if (store != null)
            {
                var storeIdArray = model.CanViewStores.Split(',');
                if (!storeIdArray.Contains(model.StoreId.ToString()))
                {
                    model.CanViewStores = model.CanViewStores.Length == 0 ? model.StoreId.ToString() : model.CanViewStores + "," + model.StoreId.ToString();
                }
            }
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
            Account entity = _db.Table.Find<Account>(model.Id);
            entity.NickName = model.NickName;
            entity.RoleId = model.RoleId;
           // entity.StoreId = model.StoreId;   // 不允许修改账号所属门店          
            entity.CanViewStores = model.CanViewStores;
            // 包含门店ID 必须检查自身门店是否存在查看权限
            if (entity.StoreId > 0)
            {
                var storeIdArray = entity.CanViewStores.Split(',');
                if (!storeIdArray.Contains(entity.StoreId.ToString()))
                {
                    entity.CanViewStores = entity.CanViewStores.Length == 0 ? entity.StoreId.ToString() : entity.CanViewStores + "," + entity.StoreId.ToString();
                }
            }
            entity.LastUpdateDate = DateTime.Now;
            _db.Update(entity);
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
