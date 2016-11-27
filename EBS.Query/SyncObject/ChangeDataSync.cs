using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBS.Query.SyncObject
{
   public class ChangeDataSync
    {
        public int Id { get; set; }

        public string DomainName { get; set; }

        public int DataId { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}
