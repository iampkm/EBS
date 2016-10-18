using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBS.Application.DTO
{
   public class RoleModel
    {
       public int Id { get; set; }
        public string Name { get; set; }

        public string Description { get; set; }

        public string MenuIds { get; set; }
    }
}
