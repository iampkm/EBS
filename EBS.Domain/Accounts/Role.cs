using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper.DBContext;
namespace EBS.Domain.Accounts
{
   public class Role:AggregateRoot<int>
    {
       public Role(string name, string description)
       {
           this.Name = name;
           this.Description = description;
           this.Items = new List<Menu>();
       }
       public string Name { get; private set; }

       public string Description { get; private set; }

       public virtual List<Menu> Items { get; private set; }

       public void AssignMenus(List<Menu> items)
       {
           this.Items = items;
       }
    }
}
