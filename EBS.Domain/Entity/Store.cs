using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBS.Domain.Entity
{
    /// <summary>
    /// 门店
    /// </summary>
   public class Store:BaseEntity
    {
       public Store() {
           this.SourceKey = Guid.NewGuid().ToString().Replace("-", "");
       }
       public string Name { get; set; }      
        public string SourceKey { get; set; }
        public string Address { get; set; }       
        public string Contact { get; set; }       
        public string Phone { get; set; }
        public DateTime CreatedOn { get; set; }
        public int CreatedBy { get; set; }
        public string Setting { get; set; }
    }
}
