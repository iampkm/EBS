using Dapper.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBS.Domain.Entity
{
    public class Category : Entity<string>
    {
        public Category()
        {
            this.Level = 1;
        }
        public string Name { get; set; }

        public string FullName { get; set; }

        public int Level { get; set; }

       
    }
}
