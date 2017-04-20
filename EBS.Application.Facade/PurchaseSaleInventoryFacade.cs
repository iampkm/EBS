using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EBS.Domain.Entity;
using EBS.Domain.Service;
using Dapper.DBContext;
using EBS.Application;
using EBS.Application.DTO;
using EBS.Application.Facade.Mapping;
using EBS.Infrastructure.Log;

namespace EBS.Application.Facade
{
    public class PurchaseSaleInventoryFacade : IPurchaseSaleInventoryFacade
    {
        IDBContext _db;
        ILogger _log;
        PurchaseSaleInventoryService _service;

        public PurchaseSaleInventoryFacade(IDBContext dbContext, ILogger log) {
            this._db = dbContext;
            this._log = log;
            _service = new PurchaseSaleInventoryService(_db, _log);
        }
        public void Generate(DateTime today)
        {
            _service.Generate(today);
        }

        public void GenerateDetail(DateTime today)
        {
            _service.GenerateDetail(today);
        }
    }
}
