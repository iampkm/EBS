using Dapper.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBS.Domain.Service
{
    public class StocktakingService
    {
        IDBContext _db;
        public StocktakingService(IDBContext dbcontext)
        {
            this._db = dbcontext;
        }


    }
}
