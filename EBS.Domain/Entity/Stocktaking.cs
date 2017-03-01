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
            this.CreatedOn = DateTime.Now;
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
        /// 创建时间
        /// </summary>
        public DateTime CreatedOn { get; set; }
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
        /// 状态
        /// </summary>

        public StocktakingStatus Status { get; set; }

        public virtual List<StocktakingItem> Items { get; set; }


        public void Cancel()
        {
            if (this.Status != StocktakingStatus.Audited)
            {
                this.Status = StocktakingStatus.Cancel;
            }
            else {
                throw new Exception("已审单据不能作废");
            }
        }
    }
}
