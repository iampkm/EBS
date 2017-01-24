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
            Status = StocktakingPlanStatus.ToBeInventory;  //初始状态
            this.CreatedOn = DateTime.Now;
            this.UpdatedOn = DateTime.Now;
        }
       
        /// <summary>
        /// 单据号
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 盘点时间
        /// </summary>
        public DateTime StocktakingDate { get; set; }

        public DateTime CreatedOn { get; set; }

        public int CreatedBy { get; set; }

        public DateTime UpdatedOn { get; set; }

        public int UpdatedBy { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public string CreatedByName { get; set; }
        /// <summary>
        /// 修改人
        /// </summary>
        public string UpdatedByName { get; set; }
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

        public List<StocktakingPlanItem> Items { get; set; }


        public void StartPlan(int editedBy,string editor)
        {
            UpdateInfo(editedBy, editor);
            this.Status = StocktakingPlanStatus.FirstInventory;
        }

        public void ChangeReplayStatus(int editedBy, string editor)
        {
            UpdateInfo(editedBy, editor);
            this.Status = StocktakingPlanStatus.Replay;
        }
        public void ChangeCompleteStatus(int editedBy, string editor)
        {
            UpdateInfo(editedBy, editor);
            this.Status = StocktakingPlanStatus.Complete;
        }

        private void UpdateInfo(int editedBy, string editor)
        {
            this.UpdatedBy = editedBy;
            this.UpdatedByName = editor;
            this.UpdatedOn = DateTime.Now;
        }
        
    }
}
