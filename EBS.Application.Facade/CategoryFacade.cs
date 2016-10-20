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
   public class CategoryFacade:ICategoryFacade
    {
       IDBContext _db;
        CategoryService _service;
        public CategoryFacade(IDBContext dbContext)
        {
            _db = dbContext;
            _service = new CategoryService(this._db);
        }
        public string Create(string parentId, string name)
        {
            var id= _service.Create(parentId, name);
            _db.SaveChange();
            return id;
        }


        public void Edit(string id, string name)
        {
            _service.Update(id, name);
            _db.SaveChange();
        }

        public void Delete(string id)
        {
            //如果该类别下有商品，不能删除

            _service.Delete(id);         
           _db.SaveChange();
        }
    }
}
