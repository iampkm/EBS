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
            Supplier entity = new Supplier()
            {
                Name = model.Name,
                Code = model.Code,
                QQ = model.QQ,
                Address = model.Address,
                AreaId = model.AreaId,
                Bank = model.Bank,
                BankAccount = model.BankAccount,
                ShortName = model.ShortName,
                LicenseNo = model.LicenseNo,
                TaxNo = model.TaxNo,
                CreatedBy = model.CreatedBy,
                UpdatedBy = model.CreatedBy,
                Contact = model.Contact,
                Phone = model.Phone,
                Type = (Domain.ValueObject.SupplierType)model.Type
            };

            _service.Create(entity);
            _db.SaveChange();
        }

        public void Edit(SupplierModel model)
        {
            Supplier entity = _db.Table.Find<Supplier>(model.Id);
            entity.Code = model.Code;
            entity.Name = model.Name;
            entity.QQ = model.QQ;
            entity.Address = model.Address;
            entity.AreaId = model.AreaId;
            entity.Bank = model.Bank;
            entity.BankAccount = model.BankAccount;
            entity.ShortName = model.ShortName;
            entity.LicenseNo = model.LicenseNo;
            entity.TaxNo = model.TaxNo;        
            entity.UpdatedBy = model.CreatedBy;
            entity.Contact = model.Contact;
            entity.Phone = model.Phone;
            entity.UpdatedOn = DateTime.Now;
            entity.Type = (Domain.ValueObject.SupplierType)model.Type;
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
