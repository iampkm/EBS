using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EBS.Query.SyncObject;
using EBS.Query.DTO;
using EBS.Query;
using EBS.Query.Service;
using Newtonsoft.Json;
using EBS.Infrastructure.Extension;
using System.Reflection;
using EBS.Application.Message;
using EBS.Application;
using EBS.Infrastructure.Events;
using Newtonsoft.Json.Converters;
using EBS.Infrastructure.Log;
namespace EBS.Admin.Controllers
{
    /// <summary>
    /// Pos 系统数据同步接口
    /// </summary>
    public class PosSyncController : Controller
    {
        IPosSyncQuery _posQuery;
        IPosSyncFacade _posFacade;
        ILogger _log;
        public PosSyncController(IPosSyncQuery query,IPosSyncFacade posFacade,ILogger log)
        {
            _posQuery = query;
            _posFacade = posFacade;
            _log = log;
        }
        public string AccountByPage(Pager page)
        {
            var result = _posQuery.QueryAccountSync(page).ToList();
            if (result.Count()==0) { return string.Empty; }
            return JsonConvert.SerializeObject(result);
        }       

        public string StoreByPage(Pager page)
        {
            var result = _posQuery.QueryStoreSync(page);
            if (result.Count() == 0) { return string.Empty; }
            return JsonConvert.SerializeObject(result);
        }       

        public string VipCardByPage(Pager page)
        {
            var result = _posQuery.QueryVipCardSync(page);
            if (result.Count() == 0) { return string.Empty; }
            return JsonConvert.SerializeObject(result);
        }       

        public string VipProductByPage(Pager page)
        {
            var result = _posQuery.QueryVipProductSync(page);
            if (result.Count() == 0) { return string.Empty; }
            return JsonConvert.SerializeObject(result);
        }       

        public string ProductByPage(Pager page,int storeId)
        {
            var result = _posQuery.QueryProductSync(page,storeId);
            if (result.Count() == 0) { return string.Empty; }
            return JsonConvert.SerializeObject(result);
        }

        public string ProductStorePriceByPage(Pager page)
        {
            var result = _posQuery.QueryProductStorePriceSync(page);
            if (result.Count() == 0) { return string.Empty; }
            return JsonConvert.SerializeObject(result);
        }

        public string ProductAreaPriceByPage(Pager page)
        {
            var result = _posQuery.QueryProductAreaPriceSync(page);
            if (result.Count() == 0) { return string.Empty; }
            return JsonConvert.SerializeObject(result);
        }

        // 事件消息处理
        public string SaleOrderSync(string body)
        {
            _log.Info("SaleOrderSync request:body={0}", body);
            _posFacade.SaleOrderSync(body);
            _log.Info("SaleOrderSync request:success");
            return "1";
        }
        public string WorkScheduleSync(string body)
        {
            _log.Info("WorkScheduleSync request:body={0}", body);
            _posFacade.WorkScheduleSync(body);
            _log.Info("SaleOrderSync request:success");
            return "1";
        }
    }
}