using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EBS.Application;
using Dapper.DBContext;
using EBS.Domain.Service;
using EBS.Domain.Entity;
using EBS.Application.DTO;
namespace EBS.Application.Facade
{
    public class StoreFacade : IStoreFacade
    {
        IDBContext _db;
        StoreService _service;
        public StoreFacade(IDBContext dbContext)
        {
            _db = dbContext;
            _service = new StoreService(this._db);
        }
        public void Create(StoreModel model)
        {
            Store entity = new Store()
            {
                Name = model.Name,
                Address = model.Address,
                Contact = model.Contact,
                CreateBy = model.CreateBy,
                CreatedOn = model.CreatedOn,
                Phone = model.Phone
            };

            _service.Create(entity); // 框架自动实现 子外键关联对象添加
            _db.SaveChange();
        }

        public void Edit(StoreModel model)
        {
            Store entity = new Store();
            entity.Id = model.Id;
            entity.Name = model.Name;
            entity.Phone = model.Phone;
            entity.Address = model.Address;
            entity.Contact = model.Contact;            
            _service.Update(entity);
            _db.SaveChange();
        }

        public void Delete(string ids)
        {
            _service.Delete(ids);
            _db.SaveChange();
        }
    }
}
