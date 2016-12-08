using EBS.Domain.ValueObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBS.Domain.Entity
{
    /// <summary>
    /// 盘点单
    /// </summary>
   public class Stocktaking:BaseEntity
    {
        public Stocktaking()
        {
            this.Items = new List<StocktakingItem>();
        }
        /// <summary>
        /// 盘点单据号
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 盘点类型
        /// </summary>
        public StocktakingType StocktakingType { get; set; }
        /// <summary>
        /// 货架码
        /// </summary>
        public string ShelfCode { get; set; }
        /// <summary>
        /// 盘点计划编号
        /// </summary>
        public int StocktakingPlanId { get; set; }
        /// <summary>
        /// 盘点计划编号
        /// </summary>
        public DateTime? CreateTime { get; set; }
        /// <summary>
        /// 创建人ID
        /// </summary>
        public int CreateBy { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public string CreateByName { get; set; }
        /// <summary>
        /// 部门ID
        /// </summary>
        public int DepartmentID { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Note { get; set; }
        /// <summary>
        /// 状态
        /// </summary>

        public StocktakingStatus Status { get; set; }

        public virtual List<StocktakingItem> Items { get; set; }
    }
}
