using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EBS.Domain.Entity;
using EBS.Application.Facade.Mapping;
using Dapper.DBContext;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using EBS.Domain.Service;
using EBS.Domain.ValueObject;
using EBS.Infrastructure.Log;
namespace EBS.Application.Facade
{
   public class PosSyncFacade:IPosSyncFacade
    {
       IDBContext _db;
        ProductService _productService;
        StoreInventoryService _storeInventoryService;
        ILogger _log;
        
        public PosSyncFacade(IDBContext dbContext,ILogger log)
        {
            _db = dbContext;
            _productService = new ProductService(_db);
            _storeInventoryService = new StoreInventoryService(_db);
            _log = log;
        }


        public void SaleOrderSync(string body)
        {
            var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" };
            var model = JsonConvert.DeserializeObject<SaleOrder>(body, dateTimeConverter);
            model.Hour = model.CreatedOn.Hour; //设置订单时段

            if (_db.Table.Exists<SaleOrder>(n => n.Code == model.Code))
            {
                _log.Info("订单{0}已存在", model.Code);
                return;
            }
            _db.Insert(model);
            _db.SaveChange();  // 先保存订单     

            if (model.Status == SaleOrderStatus.Cancel)
            {
                //作废订单只保存，不增减库存
                return;
            }
            else if (model.Status == SaleOrderStatus.Paid)
            {
                if (_db.Table.Exists<StoreInventoryHistory>(n => n.BillCode == model.Code))
                {
                    _log.Info("库存流水已经记录{0}已存在", model.Code);
                    return;
                }

                var entity = _db.Table.Find<SaleOrder>(n => n.Code == model.Code);
                var entityItems = _db.Table.FindAll<SaleOrderItem>(n => n.SaleOrderId == entity.Id).ToList();
                entity.Items = entityItems;
                if (entity.OrderType == (int)OrderType.Order)
                {
                    _storeInventoryService.StockOutSaleOrder(entity);
                }
                else
                {
                    _storeInventoryService.StockInRefundOrder(entity);
                }

                _db.SaveChange();
            }
        }


       
        public void WorkScheduleSync(string body)
        {
            var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" };
            var model = JsonConvert.DeserializeObject<WorkSchedule>(body, dateTimeConverter);
            if (_db.Table.Exists<WorkSchedule>(n => n.Code == model.Code))
            {
                _db.Update(model);
            }
            else {
                _db.Insert(model); 
            }           
            _db.SaveChange();
        }

    }
}
