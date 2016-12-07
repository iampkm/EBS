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
        ShelfService _shelfService;
        public ShelfFacade(IDBContext dbContext)
        {
            _db = dbContext;
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

        public string CreatePorduct(int storeId, int shelfLayerId, string productCodeOrBarCode, int shelfProductId)
        {
            var model = _shelfService.CreateProduct(storeId, shelfLayerId, productCodeOrBarCode, shelfProductId);
            _db.Insert(model);
            _db.SaveChange();
            return model.Code;
        }


        public void EditShelf(int id, string name)
        {
            var model= _db.Table.Find<Shelf>(id);
            model.Name = name;
            _db.Update(model);
            _db.SaveChange();
        }


        public void DeleteAll(int id,string code)
        {
            if (code.Length == 4)
            {
                _shelfService.DeleteShelf(id);
            }
            else if (code.Length == 6)
            {
                _shelfService.DeleteShelfLayer(id);
            }
            else {
                _shelfService.DeleteShelfLayerProduct(id);
            }
            _db.SaveChange();
        }

       
    }
}
