using EBS.Domain.ValueObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EBS.Infrastructure.Extension;
namespace EBS.Query.DTO
{
   public class AdjustStorePriceDto
    {
       public AdjustStorePriceDto()
       {
           this.Items = new List<AdjustStorePriceItemDto>();
       }
        public int Id { get; set; }
        public string Code { get; set; }

        public int StoreId { get; set; }
        public string StoreName { get; set; }

        public string UpdatedOn { get; set; }

        public string UpdatedByName { get; set; }

        public string CreatedByName { get; set; }

        public DateTime CreatedOn { get; set; }

        public string CreatedOnString { get {
            return this.CreatedOn.ToString("yyyy-MM-dd HH:mm:ss");
        } }

        public AdjustStorePriceStatus Status { get; set; }

        public string Remark { get; set; }

        public string AdjustStorePriceStatus
        {
            get
            {
                return Status.Description();
            }
        }

        public IEnumerable<AdjustStorePriceItemDto> Items { get; set; }
    }
}
