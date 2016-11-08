using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBS.Application.DTO
{
    public class CreateAccountModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string NickName { get; set; }
        public int RoleId { get; set; }

        public int StoreId { get; set; }
    }

   public class EditAccountModel
    {
       public int Id { get; set; }
       public string NickName { get; set; }
       public int RoleId { get; set; }
        public int StoreId { get; set; }
    }
}
