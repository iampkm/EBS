using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBS.Application.DTO
{
   public class StocktakingModel
    {
       public StocktakingModel()
        {
        }

       public int Id { get; set; }

        /// <summary>
        /// 盘点单据号
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 货架码
        /// </summary>
        public string ShelfCode { get; set; }
        /// <summary>
        /// 盘点计划编号
        /// </summary>
        public int StocktakingPlanId { get; set; }
        /// <summary>
        /// 创建人ID
        /// </summary>
        public int CreatedBy { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public string CreatedByName { get; set; }
        /// <summary>
        /// 部门ID
        /// </summary>
        public int StoreId { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Note { get; set; }
       /// <summary>
       /// 明细json 字符串
       /// </summary>
        public string ItemsJson { get; set; }

    }
}
