using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EBS.Application;
using EBS.Domain.Entity;
using Dapper.DBContext;
using EBS.Application.DTO;
using EBS.Domain.Service;
using EBS.Domain.ValueObject;
namespace EBS.Application.Facade
{
   public class BrandFacade :IBrandFacade
    {
        IDBContext _db;
        BrandService _service;
        public BrandFacade(IDBContext dbContext)
        {
            _db = dbContext;
            _service = new BrandService(this._db);
        }
        public void Create(string name)
        {
            Brand model = new Brand(){ Name = name} ;
            _service.Create(model);
            _db.SaveChange();
        }

        public void Edit(int id, string name)
        {
            Brand model = new Brand() {Id = id, Name = name };
            _service.Update(model);
            _db.SaveChange();
        }

        public void Delete(string ids)
        {
            _service.Delete(ids);
            _db.SaveChange();
        }
    }
}
