using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper.DBContext.Schema;
namespace EBS.Domain.Entity
{
   public class StoreAccount
    {
       [Key]
       public int AccountId { get; set; }

       public int StoreId { get; set; }
    }
}
