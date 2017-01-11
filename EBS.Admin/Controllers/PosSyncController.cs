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
using EBS.Application.DTO;
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
        IAccessTokenFacade _accessTokenFacade;
        public PosSyncController(IPosSyncQuery query,IPosSyncFacade posFacade,ILogger log,IAccessTokenFacade accessTokenFacade)
        {
            _posQuery = query;
            _posFacade = posFacade;
            _log = log;
            _accessTokenFacade = accessTokenFacade;
        }
        public string QueryAccount(AccessTokenModel token)
        {
            _accessTokenFacade.ValidateCDKey(token);

            var result = _posQuery.QueryAccountSync().ToList();
            if (result.Count()==0) { return string.Empty; }
            return JsonConvert.SerializeObject(result);
        }

        public string QueryStore(AccessTokenModel token)
        {
            _accessTokenFacade.ValidateCDKey(token);
            var result = _posQuery.QueryStoreSync();
            if (result.Count() == 0) { return string.Empty; }
            return JsonConvert.SerializeObject(result);
        }

        public string QueryVipCard(AccessTokenModel token)
        {
            _accessTokenFacade.ValidateCDKey(token);
            var result = _posQuery.QueryVipCardSync();
            if (result.Count() == 0) { return string.Empty; }
            return JsonConvert.SerializeObject(result);
        }

        public string QueryVipProduct(AccessTokenModel token)
        {
            _accessTokenFacade.ValidateCDKey(token);
            var result = _posQuery.QueryVipProductSync();
            if (result.Count() == 0) { return string.Empty; }
            return JsonConvert.SerializeObject(result);
        }

        public string QueryProduct(AccessTokenModel token, string productCodeOrBarCode)
        {
            _accessTokenFacade.ValidateCDKey(token);
            var result = _posQuery.QueryProductSync(token.StoreId,productCodeOrBarCode);
            if (result.Count() == 0) { return string.Empty; }
            return JsonConvert.SerializeObject(result);
        }

        public string QueryProductStorePrice(AccessTokenModel token)
        {
            _accessTokenFacade.ValidateCDKey(token);
            var result = _posQuery.QueryProductStorePriceSync(token.StoreId);
            if (result.Count() == 0) { return string.Empty; }
            return JsonConvert.SerializeObject(result);
        }

        public string QueryProductAreaPrice(AccessTokenModel token)
        {
            _accessTokenFacade.ValidateCDKey(token);
            var result = _posQuery.QueryProductAreaPriceSync(token.StoreId);
            if (result.Count() == 0) { return string.Empty; }
            return JsonConvert.SerializeObject(result);
        }

        // 事件消息处理
        public string SaleOrderSync(AccessTokenModel token,string body)
        {
            _log.Info("SaleOrderSync request:body={0}", body);
            _posFacade.SaleOrderSync(body);
            _log.Info("SaleOrderSync request:success");
            return "1";
        }

        public string WorkScheduleSync(AccessTokenModel token,string body)
        {
            _log.Info("WorkScheduleSync request:body={0}", body);
            _posFacade.WorkScheduleSync(body);
            _log.Info("WorkScheduleSync request:success");
            return "1";
        }

        public string UpdateSaleSync(AccessTokenModel token,string body)
        {
            _log.Info("UpdateSaleSync request:body={0}", body);
            _posFacade.UpdateSaleSync(body);
            _log.Info("UpdateSaleSync request:success");
            return "1";
        }




    }
}