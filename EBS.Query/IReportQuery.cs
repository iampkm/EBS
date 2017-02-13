﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EBS.Domain.Entity;
using EBS.Query.DTO;
namespace EBS.Query
{
   public interface IReportQuery
    {
       IEnumerable<PurchaseSaleInventoryDto> QueryPurchaseSaleInventorySummary(Pager page, PurchaseSaleInventorySearch condition);
    }
}
