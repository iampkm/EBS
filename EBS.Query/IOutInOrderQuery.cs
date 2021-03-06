﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EBS.Query.DTO;
namespace EBS.Query
{
   public interface IOutInOrderQuery
    {
        IEnumerable<OutInOrderDto> GetPageList(Pager page, SearchOutInOrder condition);

        OutInOrderItemDto QueryProduct(string productCodeOrBarCode, int storeId, int supplierId);

        IEnumerable<OutInOrderItemDto> QueryProductList(string inputBarCodes, int storeId, int supplierId);
        OutInOrderItemDto QueryRefundProduct(string productCodeOrBarCode, int storeId);

        IEnumerable<OutInOrderItemDto> QueryRefundProductList(string inputBarCodes, int storeId);

        OutInOrderDto GetById(int id);

        IEnumerable<OutInOrderListDto> GetFinishList(Pager page, SearchOutInOrder condition);

        IEnumerable<OutInOrderSummaryDto> GetSummaryList(Pager page, SearchOutInOrder condition);

        /// <summary>
        /// 查询其他入库单类型
        /// </summary>
        /// <returns></returns>
        IDictionary<int, string> GetOtherInOrderTypes();
        /// <summary>
        /// 查询其他出库单类型
        /// </summary>
        /// <returns></returns>
        IDictionary<int, string> GetOtherOutOrderTypes();
    }
}
