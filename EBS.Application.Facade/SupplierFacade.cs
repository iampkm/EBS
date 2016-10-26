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
                AreaId = model.AreaId,
                Bank = model.Bank,
                BankAccount = model.BankAccount,
                ShortName = model.ShortName,
                LicenseNo = model.LicenseNo,
                TaxNo = model.TaxNo,
                CreatedBy = model.CreatedBy,
                UpdatedBy = model.UpdatedBy,
                Contact = model.Contact,
                Phone = model.Phone
            };

            _service.Create(entity);
            _db.SaveChange();
        }

        public void Edit(SupplierModel model)
        {
            Supplier entity = _db.Table.Find<Supplier>(model.Id);
            entity.Name = model.Name;
            entity.AreaId = model.AreaId;
            entity.Bank = model.Bank;
            entity.BankAccount = model.BankAccount;
            entity.ShortName = model.ShortName;
            entity.LicenseNo = model.LicenseNo;
            entity.TaxNo = model.TaxNo;
            entity.CreatedBy = model.CreatedBy;
            entity.UpdatedBy = model.UpdatedBy;
            entity.Contact = model.Contact;
            entity.Phone = model.Phone;
            entity.UpdatedBy = model.UpdatedBy;
            entity.UpdatedOn = DateTime.Now;
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
