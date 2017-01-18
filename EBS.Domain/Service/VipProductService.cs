using Dapper.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EBS.Domain.Entity;
namespace EBS.Domain.Service
{
    public class VipProductService
    {
        IDBContext _db;
        public VipProductService(IDBContext dbContext)
        {
            this._db = dbContext;
        }

        public void Create(List<VipProduct> products)
        {
            List<VipProduct> insertList = new List<VipProduct>();
            List<VipProduct> updateList = new List<VipProduct>();
            foreach (var product in products)
            {               
                if (product.Id == 0)
                {
                    insertList.Add(product);
                }
                else {
                    updateList.Add(product);
                }
            }
            if (insertList.Count > 0)
            {
                _db.Insert(insertList.ToArray());
            }
            if(updateList.Count>0)
            {
                _db.Update(updateList.ToArray());
            }
        }

    }
}
