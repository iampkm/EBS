﻿using System.Collections.Generic;

namespace PaySharp.Core
{
    public interface IGateways
    {
        /// <summary>
        /// 添加网关
        /// </summary>
        /// <param name="gateway">网关</param>
        /// <returns></returns>
        bool Add(BaseGateway gateway);

        /// <summary>
        /// 获取指定网关
        /// </summary>
        /// <typeparam name="T">网关类型</typeparam>
        /// <returns></returns>
        BaseGateway Get<T>();

        /// <summary>
        /// 通过AppId获取网关
        /// </summary>
        /// <typeparam name="T">网关类型</typeparam>
        /// <param name="appId">AppId</param>
        /// <returns></returns>
        BaseGateway Get<T>(string appId);

        /// <summary>
        /// 获取网关列表
        /// </summary>
        /// <returns></returns>
        ICollection<BaseGateway> GetList();

        /// <summary>
        /// 通过storeId获取网关，通常用来对接支付接入放自己的门店
        /// </summary>
        /// <typeparam name="T">网关类型</typeparam>
        /// <param name="storeId">storeId  商户自定义门店ID</param>
        /// <returns></returns>
        BaseGateway GetByStoreId<T>(int storeId);
    }
}