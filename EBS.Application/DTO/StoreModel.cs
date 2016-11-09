using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBS.Application.DTO
{
   public class StoreModel
    {
       public StoreModel() {
           CreatedOn = DateTime.Now;
       }
       public int Id { get; set; }
        public string Name { get; set; }
        public int AreaId { get; set; }
        public string Address { get; set; }
        public string Contact { get; set; }
        public string Phone { get; set; }
        public DateTime CreatedOn { get; set; }
        public int CreateBy { get; set; }
        public string Setting { get; set; }
    }
}
