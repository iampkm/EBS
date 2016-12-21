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
namespace EBS.Application.Facade
{
   public class PosSyncFacade:IPosSyncFacade
    {
       IDBContext _db;
        ProductService _productService;
        StoreInventoryService _storeInventoryService;
        
        public PosSyncFacade(IDBContext dbContext)
        {
            _db = dbContext;
            _productService = new ProductService(_db);
            _storeInventoryService = new StoreInventoryService(_db);

        }

        public void SaleOrderSync(string body)
        {
            var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" };
            var model = JsonConvert.DeserializeObject<SaleOrder>(body, dateTimeConverter);
            model.Hour = model.CreatedOn.Hour; //设置订单时段
            _db.Insert(model);
            _db.SaveChange();  // 先保存订单
            var entity= _db.Table.Find<SaleOrder>(n => n.Code == model.Code);
            if (entity.OrderType == (int)OrderType.Order)
            {
                _storeInventoryService.StockOutSaleOrder(entity);
            }
            else {
                _storeInventoryService.StockInRefundOrder(entity);
            }
           
            _db.SaveChange();
        }

        public void InputCashAmountSync(string body)
        {
            var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" };
            var model = JsonConvert.DeserializeObject<WorkSchedule>(body, dateTimeConverter);
            var entity = _db.Table.Find<WorkSchedule>(n => n.Code == model.Code);
            if (entity == null) {
                throw new Exception("班次不存在");
            }
            entity.CashAmount = model.CashAmount;
            _db.Update(entity);
            _db.SaveChange();
        }

        public void WorkScheduleSync(string body)
        {
            var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" };
            var model = JsonConvert.DeserializeObject<WorkSchedule>(body, dateTimeConverter);
            if (_db.Table.Exists<WorkSchedule>(n => n.Code == model.Code))
            {
                throw new Exception(string.Format("{0}班次{1}已存在",model.StoreId,model.Code));
            }
            _db.Insert(model);
            _db.SaveChange();
        }
    }
}
