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
            var supplierType = entity.Type;          
            entity = model.MapTo<Supplier>(entity);
            if (model.Type != (int)supplierType)
            {
                //更改了类型，需要重新修改code
                entity.Code = _service.GenerateNewCode((int)entity.Type);
            }
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


        public void ImportProduct(string supplierProductJson,int updatedBy)
        {
            var productPriceList =JsonConvert.DeserializeObject<List<SupplierProduct>>(supplierProductJson) ;
            List<SupplierProduct> insertList = new List<SupplierProduct>();
            List<SupplierProduct> updateList = new List<SupplierProduct>();
            foreach (var product in productPriceList)
            {
                SupplierProduct model = _db.Table.Find<SupplierProduct>(n => n.ProductId == product.Id && n.SupplierId == product.SupplierId);
                if (model == null)
                {
                    product.UpdatedBy = updatedBy;
                    product.UpdatedOn = DateTime.Now;
                    insertList.Add(product);
                }
                else {
                    model.Price = product.Price;
                    model.UpdatedBy = updatedBy;
                    model.UpdatedOn = DateTime.Now;
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
        public void MarkWaitSuppply(int markId, int unMarkId,int updatedBy)
        {
            var markModel = _service.MarkWaitSupply(markId,updatedBy);
            _db.Update(markModel);
            if (unMarkId>0)
            {
                var unMarkModel = _service.UnMarkWaitSupply(unMarkId, updatedBy);
                _db.Update(unMarkModel);
            }
            _db.SaveChange();
        }

        public void UnMarkWaitSuppply(int markId, int unMarkId, int updatedBy)
        {
            var markModel = _service.ResetMark(markId, updatedBy);
            _db.Update(markModel);
            if (unMarkId>0)
            {
                var markModel2 = _service.ResetMark(unMarkId, updatedBy);
                _db.Update(markModel2); 
            }           
            _db.SaveChange();
        }


        public void removeProduct(string ids)
        {
            if (string.IsNullOrEmpty(ids)) {
                throw new Exception("请选中要删除的商品");
            }
            var idArray = ids.Split(',').ToIntArray();
            _db.Delete<SupplierProduct>(idArray);
            _db.SaveChange();
        }
    }
}
