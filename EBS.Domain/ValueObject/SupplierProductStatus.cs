using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBS.Domain.ValueObject
{
    /// <summary>
    /// 供应商商品供应状态
    /// </summary>
    public enum SupplierProductStatus
    {
        
        [Description("无合同")]
        WaitCompare = 1,
        [Description("供货中")]
        Supplying = 2,
        [Description("不供货")]
        StopSupply = 3

    }

    /// <summary>
    ///  比价状态
    /// </summary>
    public enum ComparePriceStatus
    {
        [Description("未比价")]
        WaitCompare=1,
        [Description("待候选")]
        Success=2,
        [Description("已落选")]
        Failed = 3,
        [Description("已比价")]
        HadCompared = 4
    }


}
