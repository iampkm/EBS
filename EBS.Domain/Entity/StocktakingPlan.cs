using EBS.Domain.ValueObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBS.Domain.Entity
{
   public class StocktakingPlan:BaseEntity
    {
        public StocktakingPlan()
        {
            Items = new List<StocktakingPlanItem>();
        }
       
        /// <summary>
        /// 单据号
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 盘点时间
        /// </summary>
        public DateTime StocktakingDate { get; set; }
        /// <summary>
        /// 创建人编号
        /// </summary>
        public int CreateBy { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public string CreateByName { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 修改人编号
        /// </summary>
        public int UpdateBy { get; set; }
        /// <summary>
        /// 修改人
        /// </summary>
        public string UpdateByName { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime UpdateTime { get; set; }
        /// <summary>
        /// 类型（大盘、小盘）
        /// </summary>
        public StocktakingPlanMethod Method { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public StocktakingPlanStatus Status { get; set; }
        /// <summary>
        /// 部门ID
        /// </summary>
        public int StoreId { get; set; }

        public virtual List<StocktakingPlanItem> Items { get; set; }

        public void GenerateNewCode()
        { 
            
        }
    }
}
