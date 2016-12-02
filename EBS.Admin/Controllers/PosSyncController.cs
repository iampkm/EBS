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
namespace EBS.Admin.Controllers
{
    /// <summary>
    /// Pos 系统数据同步接口
    /// </summary>
    public class PosSyncController : Controller
    {
        IPosSyncQuery _posQuery;
        IPosSyncFacade _posFacade;
        public PosSyncController(IPosSyncQuery query,IPosSyncFacade posFacade)
        {
            _posQuery = query;
            _posFacade = posFacade;
        }
        public string AccountByPage(Pager page)
        {
            var result = _posQuery.QueryAccountSync(page).ToList();
            if (result.Count()==0) { return string.Empty; }
            return JsonConvert.SerializeObject(result);
        }

        public string Account(string ids)
        {
            if (string.IsNullOrEmpty(ids)) return string.Empty;
            int[] idArray = ids.Split(',').ToIntArray();
            var result = _posQuery.QueryAccountSync(idArray);
            if (result.Count() == 0) { return string.Empty; }
            return JsonConvert.SerializeObject(result);
        }

        public string StoreByPage(Pager page)
        {
            var result = _posQuery.QueryStoreSync(page);
            if (result.Count() == 0) { return string.Empty; }
            return JsonConvert.SerializeObject(result);
        }

        public string Store(string ids)
        {
            if (string.IsNullOrEmpty(ids)) return string.Empty;
            int[] idArray = ids.Split(',').ToIntArray();
            var result = _posQuery.QueryStoreSync(idArray);
            if (result.Count() == 0) { return string.Empty; }
            return JsonConvert.SerializeObject(result);
        }

        public string VipCardByPage(Pager page)
        {
            var result = _posQuery.QueryVipCardSync(page);
            if (result.Count() == 0) { return string.Empty; }
            return JsonConvert.SerializeObject(result);
        }

        public string VipCard(string ids)
        {
            if (string.IsNullOrEmpty(ids)) return string.Empty;
            int[] idArray = ids.Split(',').ToIntArray();
            var result = _posQuery.QueryVipCardSync(idArray);
            if (result.Count() == 0) { return string.Empty; }
            return JsonConvert.SerializeObject(result);
        }

        public string VipProductByPage(Pager page)
        {
            var result = _posQuery.QueryVipProductSync(page);
            if (result.Count() == 0) { return string.Empty; }
            return JsonConvert.SerializeObject(result);
        }

        public string VipProduct(string ids)
        {
            if (string.IsNullOrEmpty(ids)) return string.Empty;
            int[] idArray = ids.Split(',').ToIntArray();
            var result = _posQuery.QueryVipProductSync(idArray);
            if (result.Count() == 0) { return string.Empty; }
            return JsonConvert.SerializeObject(result);
        }

        public string ProductByPage(Pager page)
        {
            var result = _posQuery.QueryProductSync(page);
            if (result.Count() == 0) { return string.Empty; }
            return JsonConvert.SerializeObject(result);
        }

        public string Product(string ids,int storeId)
        {
            if (string.IsNullOrEmpty(ids)) return string.Empty;
            int[] idArray = ids.Split(',').ToIntArray();
            var result = _posQuery.QueryProductSync(idArray,storeId);
            if (result.Count() == 0) { return string.Empty; }
            return JsonConvert.SerializeObject(result);
        }

        public string ChangeData(DateTime lastQueryTime)
        {
            var result = _posQuery.QueryChangeData(lastQueryTime);
            if (result.Count() == 0) { return string.Empty; }
            return JsonConvert.SerializeObject(result);
        }

        // 事件消息处理
        public string SaleOrderSync(string body)
        {
            _posFacade.SaleOrderSync(body);
            return "1";
        }
	}
}