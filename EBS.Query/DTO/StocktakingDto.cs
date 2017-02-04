using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EBS.Domain.Entity;
using EBS.Infrastructure.Extension;
using EBS.Domain.ValueObject;
namespace EBS.Query.DTO
{
   public class StocktakingDto
    {
       public StocktakingDto() {
           this.Items = new List<StocktakingItemDto>();
       }

       public int Id { get; set; }

       /// <summary>
       /// 盘点单据号
       /// </summary>
       public string Code { get; set; }
       /// <summary>
       /// 盘点类型
       /// </summary>
       public StocktakingType StocktakingType { get; set; }

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

       public StocktakingStatus Status { get; set; }
       /// <summary>
       /// 货架码
       /// </summary>
       public string ShelfCode { get; set; }

       public DateTime StocktakingDate { get; set; }

       public string StocktakingDateString
       {
           get
           {
               return StocktakingDate.ToString("yyyy-MM-dd");
           }
       }

       public string CreatedOnString { get {
           return CreatedOn.ToString("yyyy-MM-dd HH:mm:ss");
       } }

       public string StatusString
       {
           get
           {
               return Status.Description();
           }
       }

       public string StocktakingTypeString
       {
           get
           {
               return StocktakingType.Description();
           }
       }

       public string StoreName
       {
           get;
           set;
       }

       public List<StocktakingItemDto> Items { get; set; }
    }
}
