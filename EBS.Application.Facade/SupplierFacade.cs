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
using Newtonsoft.Json;
using EBS.Application.Facade.Mapping;
namespace EBS.Application.Facade
{
    public class SupplierFacade : ISupplierFacade
    {
        IDBContext _db;
        SupplierService _service;
        
        public SupplierFacade(IDBContext dbContext)
        {
            _db = dbContext;
            _service = new SupplierService(this._db);
        }
        public void Create(SupplierModel model)
        {
            Supplier entity = model.MapTo<Supplier>();
            entity.CreatedBy = model.editedBy;
            entity.UpdatedBy = model.editedBy;
            entity.Code= _service.GenerateNewCode((int)entity.Type);
            _db.Insert(entity);
            _db.SaveChange();
        }

        public void Edit(SupplierModel model)
        {
            Supplier entity = _db.Table.Find<Supplier>(model.Id);
            entity = model.MapTo<Supplier>(entity);
            entity.UpdatedBy = model.editedBy;
            entity.UpdatedOn = DateTime.Now;
            _db.Update(entity);
            _db.SaveChange();
        }

        public void Delete(string ids)
        {
           var idArray= _service.ValidateSupplierIds(ids);
           _db.Delete<Supplier>(idArray);
           _db.SaveChange();
        }


        public void ImportProduct(string supplierProductJson)
        {
            var productPriceList =JsonConvert.DeserializeObject<List<SupplierProduct>>(supplierProductJson) ;
            List<SupplierProduct> insertList = new List<SupplierProduct>();
            List<SupplierProduct> updateList = new List<SupplierProduct>();
            foreach (var product in productPriceList)
            {
                SupplierProduct model = _db.Table.Find<SupplierProduct>(n => n.ProductId == product.Id && n.SupplierId == product.SupplierId);
                if (model == null)
                {
                    insertList.Add(product);
                }
                else {
                    model.Price = product.Price;
                    updateList.Add(model);
                }
            }
            if (insertList.Count > 0) {
                _db.Insert<SupplierProduct>(insertList.ToArray());
            }
            if (updateList.Count > 0) {
                _db.Update<SupplierProduct>(updateList.ToArray());
            }           
            _db.SaveChange();
        }       
    }
}
