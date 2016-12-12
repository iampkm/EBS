using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EBS.Domain.ValueObject;
using EBS.Infrastructure.Extension;
namespace EBS.Query.DTO
{
   public class SupplierDto
    {
         public int Id { get; set; }

        public string Name { get; set; }

        public string ShortName { get; set; }

        public string Code { get; set; }
        /// <summary>
        /// 联系人
        /// </summary>
        public string Contact { get; set; }
        public string QQ { get; set; }
        public string Phone { get; set; }

        public string Bank { get; set; }
        public string BankAccount { get; set; }
        public string BankAccountName { get; set; }

        public string SupplierType { get {
                return Type.Description();
            } }

        public SupplierType Type { get; set; }

    }
}
