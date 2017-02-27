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
using System.Collections.Concurrent;
using System.Threading.Tasks;
using EBS.Infrastructure.Queue;
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
        ISimpleQueue<string> _saleQueue;
        // 订单队列
      //  private static BlockingCollection<string> _requestOrderQueue = new BlockingCollection<string>();

       
        public PosSyncController(IPosSyncQuery query,IPosSyncFacade posFacade,ILogger log,IAccessTokenFacade accessTokenFacade,ISimpleQueue<string> isaleQueue)
        {
            _posQuery = query;
            _posFacade = posFacade;
            _log = log;
            _accessTokenFacade = accessTokenFacade;
            _saleQueue = isaleQueue;            
        }
        public string QueryAccount(AccessTokenModel token)
        {
            try
            {
                _accessTokenFacade.ValidateCDKey(token);

                var result = _posQuery.QueryAccountSync().ToList();
                if (result.Count() == 0) { return string.Empty; }
                return JsonConvert.SerializeObject(result);
            }
            catch (Exception ex)
            {
                _log.Error(ex);
                return "";
            }
          
        }

        public string QueryStore(AccessTokenModel token)
        {
            try
            {
                _accessTokenFacade.ValidateCDKey(token);
                var result = _posQuery.QueryStoreSync();
                if (result.Count() == 0) { return string.Empty; }
                return JsonConvert.SerializeObject(result);
            }
            catch (Exception ex)
            {
                _log.Error(ex);
                return "";
            }
           
        }

        public string QueryVipCard(AccessTokenModel token)
        {
            try
            {
                _accessTokenFacade.ValidateCDKey(token);
                var result = _posQuery.QueryVipCardSync();
                if (result.Count() == 0) { return string.Empty; }
                return JsonConvert.SerializeObject(result);
            }
            catch (Exception ex)
            {
                _log.Error(ex);
                return "";
            }
           
        }

        public string QueryVipProduct(AccessTokenModel token)
        {
            try
            {
                _accessTokenFacade.ValidateCDKey(token);
                var result = _posQuery.QueryVipProductSync();
                if (result.Count() == 0) { return string.Empty; }
                return JsonConvert.SerializeObject(result);
            }
            catch (Exception ex)
            {
                _log.Error(ex);
                return "";
            }
           
        }

        public string QueryProduct(AccessTokenModel token, string productCodeOrBarCode)
        {
            try
            {
                _accessTokenFacade.ValidateCDKey(token);
                var result = _posQuery.QueryProductSync(token.StoreId, productCodeOrBarCode);
                if (result.Count() == 0) { return string.Empty; }
                return JsonConvert.SerializeObject(result);
            }
            catch (Exception ex)
            {
                _log.Error(ex);
                return "";
            }
          
        }

        public string QueryProductStorePrice(AccessTokenModel token)
        {
            try
            {
                _accessTokenFacade.ValidateCDKey(token);
                var result = _posQuery.QueryProductStorePriceSync(token.StoreId);
                if (result.Count() == 0) { return string.Empty; }
                return JsonConvert.SerializeObject(result);
            }
            catch (Exception ex)
            {
                _log.Error(ex);
                return "";
            }
          
        }

        public string QueryProductAreaPrice(AccessTokenModel token)
        {
            try
            {
                _accessTokenFacade.ValidateCDKey(token);
                var result = _posQuery.QueryProductAreaPriceSync(token.StoreId);
                if (result.Count() == 0) { return string.Empty; }
                return JsonConvert.SerializeObject(result);
            }
            catch (Exception ex)
            {
                _log.Error(ex);
                return "";
            }
         
        }

        // 事件消息处理
        public string SaleOrderSync(AccessTokenModel token,string body)
        {
            try
            {
                _log.Info("SaleOrderSync request:body={0}", body);
                _accessTokenFacade.ValidateCDKey(token);
               // _posFacade.SaleOrderSync(body);
                if (_saleQueue.Add(body)) //加入订单消费队列
                {
                    _log.Info("SaleOrderSync request:success");
                    return "1";
                }
                else {
                    _log.Info("SaleOrderSync request:failed");
                    return "0";
                }
            }
            catch (Exception ex)
            {
                _log.Error(ex);
                return ex.Message;
            }
           
        }

        public string WorkScheduleSync(AccessTokenModel token,string body)
        {
            try
            {
                _log.Info("WorkScheduleSync request:body={0}", body);
                _accessTokenFacade.ValidateCDKey(token);
                _posFacade.WorkScheduleSync(body);
                _log.Info("WorkScheduleSync request:success");
                return "1";
            }
            catch (Exception ex)
            {
                _log.Error(ex);
                return ex.Message;
            }
           
        }

        public string UpdateSaleSync(AccessTokenModel token,string body)
        {
            try
            {
                _log.Info("UpdateSaleSync request:body={0}", body);
                _accessTokenFacade.ValidateCDKey(token);
                _posFacade.UpdateSaleSync(body);
                _log.Info("UpdateSaleSync request:success");
                return "1";
            }
            catch (Exception ex)
            {
                _log.Error(ex);
                return ex.Message;
            }
           
        }

        public string SaleOrderInventorySync(string saleOrderCode)
        {
            try
            {
                _log.Info("SaleOrderInventorySync request:saleOrderCode={0}", saleOrderCode);
                _posFacade.SaleOrderInventorySync(saleOrderCode);
                _log.Info("SaleOrderInventorySync request:success");
                return "1";
            }
            catch (Exception ex)
            {
                _log.Error(ex);
                return ex.Message;
            }

        }

    }
}