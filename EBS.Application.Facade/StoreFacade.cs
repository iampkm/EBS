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
using EBS.Application.Facade.Mapping;
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
            //Store entity = new Store()
            //{
            //    Name = model.Name,
            //    Address = model.Address,
            //    Contact = model.Contact,
            //    CreatedBy = model.CreateBy,
            //    CreatedOn = model.CreatedOn,
            //    Phone = model.Phone
            //};
            var entity = model.MapTo<Store>();
            _service.Create(entity);
            _db.SaveChange();
        }

        public void Edit(StoreModel model)
        {
            Store entity = _db.Table.Find<Store>(model.Id);
            var oldAreaId = entity.AreaId;
            entity = model.MapTo<Store>(entity);
            _service.Update(entity, oldAreaId);
            _db.SaveChange();
        }

        public void Delete(string ids)
        {
            _service.Delete(ids);
            _db.SaveChange();
        }
    }
}
