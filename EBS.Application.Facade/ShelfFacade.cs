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
using EBS.Infrastructure.Extension;
namespace EBS.Application.Facade
{
   public class ShelfFacade:IShelfFacade
    {
        IDBContext _db;
        CategoryService _service;
        ShelfService _shelfService;
        public ShelfFacade(IDBContext dbContext)
        {
            _db = dbContext;
            _service = new CategoryService(this._db);
            _shelfService = new ShelfService(this._db);
        }

        public string CreateShelf(int storeId, string name,string code)
        {
            var model =_shelfService.CreateShelf(storeId, name, code);           
            _db.Insert(model);
            _db.SaveChange();
           return  model.Code;
        }

        public string CreateShelfLayer(int shelfId)
        {
            var model = _shelfService.CreateShelfLayer(shelfId);        
            _db.Insert(model);
            _db.SaveChange();
            return model.Code;
        }

        public string CreateProduct(int storeId, int shelfLayerId, string productCodeOrBarCode)
        {
            var model = _shelfService.CreateProduct(storeId,shelfLayerId,productCodeOrBarCode);        
            _db.Insert(model);
            _db.SaveChange();
            return model.Code;
        }
    }
}
