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
namespace EBS.Admin.Controllers
{
    /// <summary>
    /// Pos 系统数据同步接口
    /// </summary>
    public class PosSyncController : Controller
    {
        IPosSyncQuery _posQuery;
        public PosSyncController(IPosSyncQuery query)
        {
            _posQuery = query; 
        }
        public string QueryAccountSync(Pager page)
        {
            var result = _posQuery.QueryAccountSync(page);
            if (result.Count()==0) { return string.Empty; }
            return JsonConvert.SerializeObject(result);
        }

        public string QueryAccountSync(string ids)
        {
            if (string.IsNullOrEmpty(ids)) return string.Empty;
            int[] idArray = ids.Split(',').ToIntArray();
            var result = _posQuery.QueryAccountSync(idArray);
            if (result.Count() == 0) { return string.Empty; }
            return JsonConvert.SerializeObject(result);
        }

        public string QueryStoreSync(Pager page)
        {
            var result = _posQuery.QueryStoreSync(page);
            if (result.Count() == 0) { return string.Empty; }
            return JsonConvert.SerializeObject(result);
        }

        public string QueryStoreSync(string ids)
        {
            if (string.IsNullOrEmpty(ids)) return string.Empty;
            int[] idArray = ids.Split(',').ToIntArray();
            var result = _posQuery.QueryStoreSync(idArray);
            if (result.Count() == 0) { return string.Empty; }
            return JsonConvert.SerializeObject(result);
        }

        public string QueryVipCardSync(Pager page)
        {
            var result = _posQuery.QueryVipCardSync(page);
            if (result.Count() == 0) { return string.Empty; }
            return JsonConvert.SerializeObject(result);
        }

        public string QueryVipCardSync(string ids)
        {
            if (string.IsNullOrEmpty(ids)) return string.Empty;
            int[] idArray = ids.Split(',').ToIntArray();
            var result = _posQuery.QueryVipCardSync(idArray);
            if (result.Count() == 0) { return string.Empty; }
            return JsonConvert.SerializeObject(result);
        }

        public string QueryVipProductSync(Pager page)
        {
            var result = _posQuery.QueryVipProductSync(page);
            if (result.Count() == 0) { return string.Empty; }
            return JsonConvert.SerializeObject(result);
        }

        public string QueryVipProductSync(string ids)
        {
            if (string.IsNullOrEmpty(ids)) return string.Empty;
            int[] idArray = ids.Split(',').ToIntArray();
            var result = _posQuery.QueryVipProductSync(idArray);
            if (result.Count() == 0) { return string.Empty; }
            return JsonConvert.SerializeObject(result);
        }

        public string QueryProductSync(Pager page, int storeId)
        {
            var result = _posQuery.QueryProductSync(page, storeId);
            if (result.Count() == 0) { return string.Empty; }
            return JsonConvert.SerializeObject(result);
        }

        public string QueryProductSync(string ids, int storeId)
        {
            if (string.IsNullOrEmpty(ids)) return string.Empty;
            int[] idArray = ids.Split(',').ToIntArray();
            var result = _posQuery.QueryProductSync(idArray,storeId);
            if (result.Count() == 0) { return string.Empty; }
            return JsonConvert.SerializeObject(result);
        }

        public string QueryChangeData(DateTime lastQueryTime)
        {
            var result = _posQuery.QueryChangeData(lastQueryTime);
            if (result.Count() == 0) { return string.Empty; }
            return JsonConvert.SerializeObject(result);
        }
	}
}