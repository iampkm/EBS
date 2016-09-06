using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper.DBContext;
namespace EBS.Domain.Accounts
{
    public class Menu : AggregateRoot<int>
    {
        public Menu(int parentId, string name, string url, string icon)
        {
            this.ParentId = parentId;
            this.Name = name;
            this.Url = url;
            this.Icon = icon;
        }
        public int ParentId { get; private set; }
        public string Name { get; private set; }

        public string Url { get; private set; }

        public string Icon { get; private set; }
        public int DisplayOrder { get; private set; }


    }
}
