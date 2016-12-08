using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBS.Domain.ValueObject
{
   public enum StocktakingType
    {
        

        /// <summary>
        /// 盘点单
        /// </summary>
        [Description("盘点单")]
        Stocktaking = 1,
        /// <summary>
        /// 盘点修正单
        /// </summary>
        [Description("盘点修正单")]
        StocktakingCorect = 2
    }
}
